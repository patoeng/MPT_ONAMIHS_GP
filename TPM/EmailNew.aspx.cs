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
    public partial class EmailNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
            }
        }

        private void Prepare()
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_selectMEmailNew");
            if (ds.Tables.Count > 0)
            {
                var tbl = ds.Tables[0];
                var tr = new TableHeaderRow {TableSection = TableRowSection.TableHeader};
                for (int i = 1; i < tbl.Columns.Count; i++)
                {
                    var tc2 = new TableHeaderCell {Text = tbl.Columns[i].ColumnName};
                    tr.Controls.Add(tc2);
                }
                tblList.Rows.Add(tr);

                foreach (DataRow dr in tbl.Rows)
                {
                    var tr2 = new TableRow {TableSection = TableRowSection.TableBody};
                    for (int i = 1; i < tbl.Columns.Count; i++)
                    {
                        var tc = new TableCell { Text = dr[i].ToString()};
                        if (i == 3)
                        {
                            tc.Attributes.Add("id", "lbl"+tbl.Columns[i].ColumnName + "_" + dr[0]);
                            tc.Attributes.Add("class", "editableYesNo");
                        }
                        tr2.Controls.Add(tc);
                    }
                    tblList.Rows.Add(tr2);
                }
            }
        }
    }
}