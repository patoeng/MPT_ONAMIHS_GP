using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using System.Web.UI.HtmlControls;

namespace TPM
{
    public partial class ASMODAttribute : System.Web.UI.Page
    {
        public TPMHelper F = new TPMHelper();
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (session.IsAdministrator)
                {
                    prepare();
                }
                else
                {
                    Server.Transfer(session.Redirection);
                }


            }
        }
        protected void prepare(){
            DataSet ds = SqlHelper.ExecuteDataset(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_MAssetModels_attribAll");
            DataTable dt = ds.Tables[0];
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            List<string> thead = new List<string>();
            thead.Add("ID");
            thead.Add("SHORT NAME");
            thead.Add("DESCRIPTIONS");  
            thead.Add("CHECKLIST DAILY PROMPT");
            thead.Add("CHECKLIST MONTHLY");
            thead.Add("BOM");

            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            for (int i = 0; i < thead.Count; i++) {
                tc = new TableHeaderCell();
                tc.Text = thead[i];
                tc.Style.Add("text-align", "center");
                tc.Style.Add("vertical-align", "middle");
                if ((i == 3) ||(i==4)){ tc.ColumnSpan = 5; } else { tc.RowSpan = 2; }
                tr.Cells.Add(tc);
            }
           
            tblContainer.Rows.Add(tr);
           
                thead.Clear();
                thead.Add("PREVIEW");
                thead.Add("DETAILS");
                thead.Add("ITEMS");
                thead.Add("IMAGES");

                thead.Add("CREATE NEW");
                tr = new TableRow();
                for (int y = 0; y < 2; y++)
                {
                    tr.TableSection = TableRowSection.TableHeader;
                    for (int i = 0; i < thead.Count; i++)
                    {
                        tc = new TableHeaderCell();
                        tc.Style.Add("text-align", "center");
                        tc.Style.Add("vertical-align", "middle");
                        tc.Text = thead[i];
                        tr.Cells.Add(tc);
                    }
                }
                tblContainer.Rows.Add(tr);
                
            
            HtmlGenericControl htmTag = new HtmlGenericControl("a");
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                for (int h = 0; h < 2; h++)
                {
                    int i;
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        tc = new TableCell();
                        if (i >=3)
                        {
                            htmTag = new HtmlGenericControl("a");
                            htmTag.InnerHtml = "View";
                            htmTag.Attributes.Add("href", "/"+TPMHelper.WebDirectory+"/Mchecklist.aspx?m=p&id=" + dr[i].ToString());
                            //pre

                            if (dr[i].ToString() == "")
                            {
                                tc.Text = "NA";
                            }
                            else { tc.Controls.Add(htmTag); }
                        }
                        else
                        {
                            tc.Text = dr[i].ToString();
                        }
                        if (((h == 1) && (i < 4)) || ((h == 0) && (i == 4))) { }
                        else
                        {
                            tr.Cells.Add(tc);
                        }
                    }


                    

                    tc = new TableCell();
                    htmTag = new HtmlGenericControl("a");
                            htmTag.InnerHtml = "View";
                            htmTag.Attributes.Add("href", "/"+TPMHelper.WebDirectory+"/Ycrud.aspx?table=MCheckLists&d=AssetModel_Id&v=" + dr[0].ToString());
                            //pre

                            if (dr[i-2+h].ToString() == "")
                            {
                                tc.Text = "NA";
                            }
                            else { tc.Controls.Add(htmTag); }
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    htmTag = new HtmlGenericControl("a");
                            htmTag.InnerHtml = "View";
                            htmTag.Attributes.Add("href", "/"+TPMHelper.WebDirectory+"/Ycrud.aspx?table=MListItems&d=checklist_Id&v=" + dr[i - 2 + h].ToString());
                            //pre

                            if (dr[i-2+h].ToString() == "")
                            {
                                tc.Text = "NA";
                            }
                            else { tc.Controls.Add(htmTag); }
                    tr.Cells.Add(tc);

                    tc = new TableCell();

                    htmTag = new HtmlGenericControl("a");
                    htmTag.InnerHtml = "View";
                    htmTag.Attributes.Add("href", "/" + TPMHelper.WebDirectory + "/YCheckListImg.aspx?i=" + dr[3 + h].ToString());
                    //pre

                    if (dr[3 + h].ToString() == "")
                    {
                        tc.Text = "NA";
                    }
                    else { tc.Controls.Add(htmTag); }

                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    htmTag = new HtmlGenericControl("a");
                    htmTag.InnerHtml = "New";
                    htmTag.Attributes.Add("href", "/" + TPMHelper.WebDirectory + "/Ycrud.aspx?table=MCheckLists&d=AssetModel_Id&v=" + dr[0].ToString());
                    //pre
                     tc.Controls.Add(htmTag);
                    tr.Cells.Add(tc);

                }   
                htmTag = new HtmlGenericControl("a");
                htmTag.InnerHtml = "View";
                htmTag.Attributes.Add("href", "/" + TPMHelper.WebDirectory + "/YMBOMS.aspx?a=" + dr[0].ToString());
                //pre
                tc = new TableCell();
                tc.Controls.Add(htmTag);

                tr.Cells.Add(tc);

                tblContainer.Rows.Add(tr);
            }
        }
    }
}