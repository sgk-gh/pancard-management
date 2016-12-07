using System;

public partial class _Default : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
    }
}