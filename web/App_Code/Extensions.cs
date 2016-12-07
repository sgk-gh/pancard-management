using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Extensions
/// </summary>
public static class Extensions
{
    public static string UploadFile(this System.Web.UI.WebControls.FileUpload file, string directorypath)
    {
        if (!file.HasFile) return null;
        var fileName = Guid.NewGuid() + "_" + file.FileName;
        file.SaveAs(directorypath + fileName);
        return fileName;
    }
    public static string UploadFile(this System.Web.UI.WebControls.FileUpload file, string directorypath, string filename)
    {
        if (!file.HasFile) return null;
        if (filename.Trim().Length == 0) return file.UploadFile(directorypath);
        file.SaveAs(directorypath +  filename);
        return filename;
    }

    public static void BindGridView(this System.Web.UI.WebControls.GridView gridView, object source, int pageSize, int pageIndex)
    {
        gridView.DataSource = source;
        gridView.PageSize = pageSize;
        gridView.PageIndex = pageIndex;
        gridView.DataBind();
    }
}
