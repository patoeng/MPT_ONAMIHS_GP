using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class ToolShow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowList();
            }
        }

        private void ShowList()
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBToolSstring, CommandType.StoredProcedure, "usp_ToolStock");
            if (ds.Tables.Count > 0)
            {
                var tbl = ds.Tables[0];
                var tr = new TableHeaderRow { TableSection = TableRowSection.TableHeader };
                for (int i = 1; i < tbl.Columns.Count; i++)
                {
                    var tc2 = new TableHeaderCell { Text = tbl.Columns[i].ColumnName };
                    tr.Controls.Add(tc2);
                }
                tblTool.Rows.Add(tr);

                foreach (DataRow dr in tbl.Rows)
                {
                    var tr2 = new TableRow { TableSection = TableRowSection.TableBody };
                    for (int i = 1; i < tbl.Columns.Count; i++)
                    {
                        var tc = new TableCell { Text = dr[i].ToString() };
                            tc.Attributes.Add("id", "lbl" + tbl.Columns[i].ColumnName.Replace(' ','_') + "__" + dr[0]);
                        switch (i)
                        {
                            case 2: tc.Attributes.Add("class", "editableDescription");
                                break;
                            case 3: tc.Attributes.Add("class", "editableMachine");
                                break;
                            case 5: tc.Attributes.Add("class", "editableToolLifeSpec");
                                break;
                            case 7: tc.Attributes.Add("class", "editablePosition");
                                break;
                        }
                            
                        
                        tr2.Controls.Add(tc);
                    }
                    tblTool.Rows.Add(tr2);
                }
            }
        }
    }
}