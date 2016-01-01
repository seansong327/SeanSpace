function ajaxPost(url, param, callBack) {
    var xmlHttpReq = null;  //声明一个空对象用来装入XMLHttpRequest

    if (window.ActiveXObject) {//IE5 IE6是以ActiveXObject的方式引入XMLHttpRequest的
        xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else if (window.XMLHttpRequest) {//除IE5 IE6 以外的浏览器XMLHttpRequest是window的子对象
        xmlHttpReq = new XMLHttpRequest();
    }

    if (xmlHttpReq != null) {
        xmlHttpReq.open("POST", url, true); //调用open()方法并采用异步方式
        xmlHttpReq.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xmlHttpReq.onreadystatechange = function () {
            if (xmlHttpReq.readyState == 4 && xmlHttpReq.status == 200) {
                callBack(xmlHttpReq.responseText);
            }
        }; //设置回调函数

        xmlHttpReq.send(param);
    }
}

function setHtml(id, html) {
    document.getElementById(id).innerHTML = html;
}

function getValue(id) {
    return document.getElementById(id).value;
}

function setValue(id, val) {
    document.getElementById(id).value = val;
}

function showBlock(id) {
    document.getElementById(id).style.display = "block";
}

function showInline(id) {
    document.getElementById(id).style.display = "inline";
}

function hide(id) {
    document.getElementById(id).style.display = "none";
}