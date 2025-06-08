using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriculturalRecycle.Layout
{
    public class AutoSizeFormClass
    {
        public struct controlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        public List<controlRect> oldCtrl = new List<controlRect>();
        int ctrlNo = 0; 
        public void controllInitializeSize(Control mForm)
        {
            controlRect cR;
            cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
            oldCtrl.Add(cR); 
            AddControl(mForm); 
        }
        private void AddControl(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {
                controlRect objCtrl;
                objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                oldCtrl.Add(objCtrl);
                if (c.Controls.Count > 0)
                    AddControl(c);
            }
        }
        public void controlAutoSize(Control mForm)
        {
            if (ctrlNo == 0)
            {
                controlRect cR;
                cR.Left = 0; cR.Top = 0; cR.Width = mForm.PreferredSize.Width; cR.Height = mForm.PreferredSize.Height;

                oldCtrl.Add(cR); 
                AddControl(mForm); 
            }
            float wScale = (float)mForm.Width / (float)oldCtrl[0].Width; 
            float hScale = (float)mForm.Height / (float)oldCtrl[0].Height; 
            ctrlNo = 1; 
            AutoScaleControl(mForm, wScale, hScale); 
        }
        private void AutoScaleControl(Control ctl, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            foreach (Control c in ctl.Controls)
            {
                ctrLeft0 = oldCtrl[ctrlNo].Left;
                ctrTop0 = oldCtrl[ctrlNo].Top;
                ctrWidth0 = oldCtrl[ctrlNo].Width;
                ctrHeight0 = oldCtrl[ctrlNo].Height;
                c.Left = (int)(ctrLeft0 * wScale); 
                c.Top = (int)((ctrTop0) * hScale); 
                c.Width = (int)(ctrWidth0 * wScale); 
                c.Height = (int)(ctrHeight0 * hScale); 
                ctrlNo++; 
                if (c.Controls.Count > 0)
                    AutoScaleControl(c, wScale, hScale);


                if (ctl is DataGridView)
                {
                    DataGridView dgv = ctl as DataGridView;
                    Cursor.Current = Cursors.WaitCursor;

                    int widths = 0;
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells); 
                        widths += dgv.Columns[i].Width; 
                    }
                    if (widths >= ctl.Size.Width) 
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; 
                    else
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 


                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
