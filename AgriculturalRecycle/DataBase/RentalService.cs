using System;
using MySql.Data.MySqlClient;

namespace AgriculturalRecycle
{
    internal static class RentalService
    {
        public static string Borrow(int borrowerId, int ownerId, int equipmentId, int productId,
            object listingDate, string duration, decimal monthlyRent, decimal deposit, int months, DateTime today)
        {
            if (monthlyRent <= 0 || deposit < 0)
                return "设备的租金或押金设置无效。";

            decimal rent = monthlyRent * months;
            using (var connection = DBhelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new MySqlCommand("", connection, transaction))
                {
                    command.Parameters.AddWithValue("@borrowerId", borrowerId);
                    command.Parameters.AddWithValue("@ownerId", ownerId);
                    command.Parameters.AddWithValue("@equipmentId", equipmentId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@listingDate", listingDate);
                    command.Parameters.AddWithValue("@duration", duration);
                    command.Parameters.AddWithValue("@monthlyRent", monthlyRent);
                    command.Parameters.AddWithValue("@rent", rent);
                    command.Parameters.AddWithValue("@deposit", deposit);
                    command.Parameters.AddWithValue("@total", rent + deposit);
                    command.Parameters.AddWithValue("@borrowDate", today);
                    command.Parameters.AddWithValue("@expectedReturn", today.AddMonths(months));

                    command.CommandText = @"DELETE FROM borrowequipment
                        WHERE EquipmentID = @equipmentId AND ProductID = @productId AND UserID = @ownerId
                            AND AddDate <=> @listingDate AND RentalDuration = @duration
                            AND Rental = @monthlyRent AND COALESCE(Deposit, 0) = @deposit
                        LIMIT 1";
                    if (command.ExecuteNonQuery() != 1)
                        return "这条出租信息已变更或下架，请重新选择。";

                    command.CommandText = @"UPDATE userinfo SET Balance = Balance - @total
                        WHERE UserID = @borrowerId AND Balance >= @total";
                    if (command.ExecuteNonQuery() != 1)
                        return "余额不足，无法支付租金和押金。";

                    command.CommandText = "UPDATE userinfo SET Balance = Balance + @total WHERE UserID = @ownerId";
                    if (command.ExecuteNonQuery() != 1)
                        return "找不到设备出租方。";

                    command.CommandText = @"INSERT INTO borrowrecords
                        (EquipmentID, UserID, BorrowDate, ExpectedReturn, Deposit, Rental, PossessorID)
                        VALUES (@equipmentId, @borrowerId, @borrowDate, @expectedReturn, @deposit, @rent, @ownerId)";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    return null;
                }
            }
        }

        public static bool Return(int borrowerId, int recordId)
        {
            using (var connection = DBhelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new MySqlCommand("", connection, transaction))
                {
                    command.Parameters.AddWithValue("@borrowerId", borrowerId);
                    command.Parameters.AddWithValue("@recordId", recordId);
                    command.CommandText = @"SELECT EquipmentID, PossessorID FROM borrowrecords
                        WHERE UserID = @borrowerId AND RecordID = @recordId FOR UPDATE";

                    int equipmentId;
                    int ownerId;
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return false;
                        equipmentId = reader.GetInt32(0);
                        ownerId = reader.GetInt32(1);
                    }

                    command.Parameters.AddWithValue("@equipmentId", equipmentId);
                    command.Parameters.AddWithValue("@ownerId", ownerId);
                    // Serialize returns to the same owner's inventory.
                    command.CommandText = "SELECT UserID FROM userinfo WHERE UserID = @ownerId FOR UPDATE";
                    if (command.ExecuteScalar() == null)
                        return false;

                    command.CommandText = @"UPDATE userequipment SET Count = Count + 1
                        WHERE UserID = @ownerId AND EquipmentID = @equipmentId";
                    int updated = command.ExecuteNonQuery();
                    if (updated > 1)
                        return false;

                    if (updated == 0)
                    {
                        command.CommandText = @"INSERT INTO userequipment (UserID, EquipmentID, Count)
                            VALUES (@ownerId, @equipmentId, 1)";
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "DELETE FROM borrowrecords WHERE UserID = @borrowerId AND RecordID = @recordId";
                    if (command.ExecuteNonQuery() != 1)
                        return false;

                    transaction.Commit();
                    return true;
                }
            }
        }
    }
}
