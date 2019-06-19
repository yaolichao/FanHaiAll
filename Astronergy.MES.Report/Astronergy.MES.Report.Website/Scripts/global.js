//去空格函数
//用法： var str = "  hello "; alert(str.trim());
String.prototype.trim = function()
{
    return this.replace(/(^[\\s]*)|([\\s]*$)/g, "");
}
String.prototype.lTrim = function()
{
    return this.replace(/(^[\\s]*)/g, "");
}
String.prototype.rTrim = function()
{
    return this.replace(/([\\s]*$)/g, "");
}

//弹出消息
function showAlert(message, returnUrl)
{        
    alert(message);    
    if (returnUrl && returnUrl != "")
         window.location.href = returnUrl;
}

function showPage(title, pageUrl, width, height, isModal)
{ 
    $(function(){ 
       
        if (pageUrl.indexOf('?') >0)
            tb_show(title, pageUrl + "&KeepThis=true&TB_iframe=true&width=" + width + "&height=" + height + "&modal=" + isModal, null);
        else
            tb_show(title, pageUrl + "?KeepThis=true&TB_iframe=true&width=" + width + "&height=" + height + "&modal=" + isModal, null);
    });
}

//全选/取消全选
function selectAll(involker)
{ 
    var inputElements = document.getElementsByTagName('input');

    for (var i = 0 ; i < inputElements.length ; i++)
    {
        var myElement = inputElements[i];
        
        if (myElement.type === "checkbox" && myElement.id.indexOf('chkThis') > 0)
            myElement.checked = involker.checked;

    }
}

//多选删除确认
function confirmPop(msg)
{
    var frm = document.forms[0];

    var j = 0;
    for (i=0; i<frm.length; i++)
    {
        if (frm.elements[i].name.indexOf('chkThis') > 0)
        {
            if(frm.elements[i].checked) 
            {
                j += 1;
                return confirm(msg);
            }
        }
    }
    
    if (j == 0)
    {
        //alert("您至少需要选择一条记录！");
        return false;
    }
}

function checkAll(name)
{
    var objs = document.getElementsByTagName("input");  
    for(var i=0; i<objs.length; i++)
    {  
        if(objs[i].type.toLowerCase() == "checkbox" )  
        {
            if (objs[i].name.indexOf(name) >= 0)
                objs[i].checked = true; 
        }
    }    
}

function uncheckAll(name)
{
    var objs = document.getElementsByTagName("input");  
    for(var i=0; i<objs.length; i++)
    {  
        if(objs[i].type.toLowerCase() == "checkbox" )  
        {
            if (objs[i].name.indexOf(name) >= 0)
                objs[i].checked = false; 
        }
    }          
}
//GridView 确定是否需要导出
   function IsEnableExcelReport(strBool)
   {
       var ibtnExport=document.getElementById("ctl00_ContentPlaceHolder1_ibtnExport");
      if(strBool)
      {
          if(ibtnExport!=null)
          {
             ibtnExport.href="#";
          }
      }
   }
   var tb_pathToImage = "/Images/loadingAnimation.gif";
   //缓冲页面设定
    function viewDivMethod()
    {
    imgLoader = new Image();// preload image
	imgLoader.src = tb_pathToImage;
	
        var createDiv=document.createElement("div");
        createDiv.style.border = "1px solid #ccc";
        createDiv.setAttribute("id","divView");
       
        var img=document.createElement("img");
        img.setAttribute("scr",tb_pathToImage);
        createDiv.appendChild(img);

        var tlink = document.getElementById("divPnl");
        tlink.appendChild(createDiv);
    }

    function removeDivMethod()
    {
        var divView=document.getElementById("divView");
        var tlink = document.getElementById("divPnl");
        if(divView!=null||divView!=undefined)
        {
            tlink.removeChild(divView);
         }
    }
