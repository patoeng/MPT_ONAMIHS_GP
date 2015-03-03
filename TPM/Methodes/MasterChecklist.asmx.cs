using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for MasterChecklist
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class MasterChecklist : System.Web.Services.WebService
    {
        public class ChecklistItem
        {
            public int Id { get; set; }
            public string Number { get; set; }
            public string ItemCheck { get; set; }
            public string Standard { get; set; }
            public string Method { get; set; }
            public string Category { get; set; }
        }

        public class ChecklistImage
        {
            public int Id { get; set; }
            public string Link { get; set; }
        }

        public class ChecklistForm
        {
            public ChecklistForm()
            {
                //ChecklistImages = new List<ChecklistImage>();
                //ChecklistItems = new List<ChecklistItem>();
            }
            public int Id { get; set; }
            public string Revision { get; set; }
            public string FormNumber { get; set; }
            public string PreparedBy { get; set; }
            public string ApprovedBy { get; set; }
            public string DeptID { get; set; }
            public string AMI { get; set; }
           // public List<ChecklistItem> ChecklistItems { get; set; }
           // public List<ChecklistImage> ChecklistImages { get; set; }
            public string ChecklistItemsTbl { get; set; }
            public string ChecklistImagesTbl { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetMachineModel(string deptId)
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_MAssetModelsSelect_byDept", new SqlParameter("@deptid", deptId));
            var row = new Dictionary<string, string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    row.Add(dr[0].ToString(), dr[1].ToString());


                }
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(row);
            return s;
           
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string ChecklistItemUpdate(string id, string value,string orig)
        {
            /**
                @id int=NULL,
                @Standard nvarchar(max)=NULL,
                @Methode nvarchar(max)=NULL,
                @Category nvarchar(max)=NULL,
                @Descriptions varchar(MAX)=NULL,
                @Picture_Legend varchar(10)=NULL
             */
            var pr = id.Split('-');
            var par = new List<SqlParameter>
                {
                    new SqlParameter("@id",pr[0]),
                    new SqlParameter("@"+pr[1],value)
                };
            try
            {
                SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MListItemsUpdate2",
                                      par.ToArray());
                return value;
            }
            catch (Exception)
            {

                return orig;
            }
            
           
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string ChecklistItemInsert(string cid, string id, string value, string orig)
        {
            /**
                @id int=NULL,
                @Standard nvarchar(max)=NULL,
                @Methode nvarchar(max)=NULL,
                @Category nvarchar(max)=NULL,
                @Descriptions varchar(MAX)=NULL,
                @Picture_Legend varchar(10)=NULL
             */
            var pr = id.Split('-');
            string exception="";
            var par = new List<SqlParameter>
                {
                    new SqlParameter("@checklist_id",cid),
                    new SqlParameter("@"+pr[1],value)
                };
            var Id = new SqlParameter
                {
                    Direction = ParameterDirection.InputOutput,
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Value = cid == string.Empty ? null : cid
                };
            par.Add(Id);
            string k = "";
            try
            {
                SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MListItemsInsert2",
                                      par.ToArray());
                k= value;
            }
            catch (Exception e)
            {

                k= orig;

                exception = e.Message + ">>>>>" + e.StackTrace;
            }
            var ot = new Dictionary<string, string>
                {
                    {"newListId", pr[0].ToString()},
                    {"InsertedId", Id.Value.ToString()},
                    {"value", k},
                    {"error", exception}
                };
            var json = new JavaScriptSerializer();
            string s = json.Serialize(ot);
            return s;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string ChecklistUpdate(string cid, string id, string value, string orig, string ctid, string amid, string dpid, string sn)
        {
            /* @id int=NULL output,
	            @asset_id int=NULL,
                @CheckListType_Id int=NULL,
                @Prepared_By varchar(50)=NULL,
                @Approved_By varchar(50)=NULL,
                @Form_Number varchar(50)=NULL,
                @Revision varchar(50)=NULL*/
            var par = new List<SqlParameter>
                {
                    new SqlParameter("@CheckListType_Id", ctid),
                    new SqlParameter("@assetmodel_id", amid),
                    new SqlParameter("@department_id", dpid),
                    new SqlParameter("@SerialNumber",sn.Trim())
                };
            var Id = new SqlParameter
                {
                    Direction = ParameterDirection.InputOutput,
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Value = cid==string.Empty? null : cid
                };
            par.Add(Id);
            switch (id.ToLower())
            {
                case "lblform": par.Add(new SqlParameter("@Form_Number",value));
                    break;
                case "lblapproved_by": par.Add(new SqlParameter("@Approved_By", value));
                    break;
                case "lblprepared_by": par.Add(new SqlParameter("@Prepared_By", value));
                    break;
                case "lblrevision_no": par.Add(new SqlParameter("@Revision", value));
                    break;
            }

            int i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_MCheckListsUpdate2", par.ToArray());
            var res = new Dictionary<string, string> {{"CID", Id.Value.ToString()}, {"VAL", i > 0 ? orig : value}};
            var json = new JavaScriptSerializer();
            string s = json.Serialize(res);
            return s;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string deleteItem(string table, string id)
        {
            var usp = "";
            switch (table.ToLower())
            {
                case "image":
                    usp = "usp_MCheckListImagesDelete";
                        break;
                case "listitem":
                        usp = "usp_mlistitemsdelete";
                        break;
            }
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure, usp,
                                                  new SqlParameter("@id", id));
            return i<0? "":"false";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetCheckList(string deptId="", string id= "", string cid = null, string sn = "")
        {
            /*
             *  @DepartmentId INT=NULL,
             *  @AssetModelId INT=NULL,
             *  @CheckListTypeId INT = NULL
             * */
            var dep = new SqlParameter
                {
                    Direction = ParameterDirection.InputOutput,
                    ParameterName = "@DepartmentId",
                    SqlDbType = SqlDbType.BigInt,
                    Value = deptId
                };
            var ami = new SqlParameter
            {
                Direction = ParameterDirection.InputOutput,
                ParameterName = "@AssetModelId",
                SqlDbType = SqlDbType.BigInt,
                Value = id
            };
            var par = new List<SqlParameter>
                {
                    dep,
                    ami,
                    new SqlParameter("@CheckListTypeId", cid),
                    new SqlParameter("@Serialnumber",sn)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_getMasterCheckList2", par.ToArray());
            var row = new ChecklistForm();
            var dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                row.Id = Convert.ToInt32(dr["id"]);
                row.PreparedBy = dr["Prepared_By"].ToString();
                row.ApprovedBy = dr["Approved_By"].ToString();
                row.FormNumber = dr["Form_Number"].ToString();
                row.Revision = dr["revision"].ToString();
            }
            row.DeptID = (dep.Value.ToString());
            row.AMI = (ami.Value.ToString());

            var tbl = new Table
                {
                    ID="TblListItems",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-striped table-bordered"
                };
           
           var dt2 = ds.Tables[1];
           
            var rw = new TableRow{TableSection = TableRowSection.TableHeader};
            var thc = new TableHeaderCell();
            for (int j = 0; j < dt2.Columns.Count - 1; j++)
            {
                thc = new TableHeaderCell {Text = dt2.Columns[j].ColumnName};
                rw.Cells.Add(thc);
            }
            tbl.Rows.Add(rw);

            foreach (DataRow dr in dt2.Rows)
                {
                    /*row.ChecklistItems.Add(new ChecklistItem
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Number = dr["Picture_Legend"].ToString(),
                            Category = dr["Category"].ToString(),
                            ItemCheck = dr["Descriptions"].ToString(),
                            Method = dr["Methode"].ToString(),
                            Standard = dr["Standard"].ToString()
                        });*/
                    rw = new TableRow
                        {
                            TableSection = TableRowSection.TableBody,
                            ID = dr["id"]+"-row",
                            ClientIDMode = ClientIDMode.Static

                        };
                    for (int i = 0; i < dt2.Columns.Count - 1; i++)
                    {
                        var tc = new TableCell
                            {
                                ID = dr[dt2.Columns.Count-1] + "-" + dt2.Columns[i].ColumnName,
                                ClientIDMode = ClientIDMode.Static,
                                CssClass = dt2.Columns[i].ColumnName=="DELETE"? "" : "editable_cli",
                                Text = (dr[i].ToString())
                            };
                        rw.Cells.Add(tc);
                    }
                    tbl.Rows.Add(rw);
                }
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            row.ChecklistItemsTbl = sw.ToString();

            var htm = new HtmlGenericControl("h4");
            htm.Attributes.Add("class", "btn btn-primary");
            htm.Attributes.Add("id", "add_list");
            htm.InnerText = "ADD NEW ITEM";

            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            htm.RenderControl(htw);
            row.ChecklistItemsTbl = sw + "<p/>" + row.ChecklistItemsTbl;

            tbl = new Table
            {
                ID = "TblImage",
                ClientIDMode = ClientIDMode.Static,
                CssClass = "table table-striped table-bordered"
            };
   
            dt = ds.Tables[2];
            foreach (DataRow dr in dt.Rows)
            {
                const int imagewidth = 250;
                string filename = "/"+TPMHelper.WebDirectory+"/UploadedFiles/" + dr["Descriptions"];
                var img = new Image
                    {
                        ID = dr["id"] + "-image",
                        ClientIDMode = ClientIDMode.Static,
                        CssClass =  "editable_image",
                        ImageUrl = filename,
                        AlternateText =  filename
                    };

                try
                {
                    var img2 = System.Drawing.Image.FromFile(Server.MapPath(filename));
                    float ratio = img2.Width/imagewidth;
                    float height = img2.Height/ratio;
                    img.Width = imagewidth;
                    img.Height = (int) height;
                }
                catch (Exception ex)
                {
                    
                }

                var tc = new TableCell();
                tc.Style.Add("vertical-align", "middle");
                tc.Style.Add("text-align", "center");
                tc.Style.Add("width","400px!important");
                tc.Controls.Add(img);
                rw = new TableRow {
                            TableSection = TableRowSection.TableBody,
                            ID = dr["id"]+"-row2",
                            ClientIDMode = ClientIDMode.Static
                };
                rw.Cells.Add(tc);
                tc = new TableCell();
                tc.Style.Add("vertical-align", "middle");
                tc.Style.Add("text-align", "center");
                tc.Text = dr["delete"].ToString();
                rw.Cells.Add(tc);
                tbl.Rows.Add(rw);
            }
            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            row.ChecklistImagesTbl = sw.ToString();

           
            
            htm = new HtmlGenericControl("h4");
            htm.Attributes.Add("class", "btn btn-primary");
            htm.Attributes.Add("id", "add_image");
            htm.InnerText = "ADD NEW IMAGE";
            

            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            htm.RenderControl(htw);
            row.ChecklistImagesTbl = sw + "<p/>" + row.ChecklistImagesTbl;

          

            var json = new JavaScriptSerializer();
            string s = json.Serialize(row);
            return s;
        
        }
    }
}
