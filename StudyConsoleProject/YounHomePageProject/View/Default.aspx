<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="YounHomePageProject.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Youn Home</title>
    <script type="text/javascript">

        function OnLoad() {
            var date = new Date(92, 8, 20).toDateString();
            document.getElementById("txtbirthDate").value = date;
        }

        function GetFisrstCreateDate() {

        }

        function OpenCalculator() {
            var objCalc = document.getElementById("tblCalc");
            if (objCalc.style.visibility == "hidden") {
                objCalc.style.visibility = "visible";
            } else {
                objCalc.style.visibility = "hidden";
            }
        }

        function Calculate(calcValue) {
            var objResultText = document.getElementById("txtResult");
            objResultText.value += calcValue;
        }

        function Clear() {
            document.getElementById("txtResult").value = "";
        }

        function Result() {
            var objReusltText = document.getElementById("txtResult");
            objReusltText.value = eval(objReusltText.value);
         }
    </script>
</head>
<body onload="OnLoad();">
    <form id="form1" runat="server">
    <div id="">
        최초생성일
        <input type="text" id="txtFirstCreateDate" readonly="readonly" />
        <br />
        홈페이지가 오픈한지
        <input type="text" id="txtOpenRecordDate" readonly="readonly" />일이 지났습니다.
        <br />
        제작자 생일 :
        <input type="text" id="txtbirthDate" readonly="readonly"/>
        <br />
        <input type="button" id="btnCalculatorOpen" onclick="OpenCalculator();" value="계산기" />
        <br />
        <!--계산기-->
        <table id="tblCalc" style="visibility:hidden">
        <tr>
        <td><input type="text" id="txtResult" /></td>
        </tr>
        <tr>
        <td>
        <input type="button" id="btnSeven" value="7" onclick="Calculate('7');"/>
        <input type="button" id="btnEight" value="8" onclick="Calculate('8');"/>
        <input type="button" id="btnNine" value="9" onclick="Calculate('9');"/>
        <input type="button" id="btnSum" value=" + " onclick="Calculate('+');"/>
        </td>
        </tr>
        <tr>
        <td>
        <input type="button" id="btnFour" value="4" onclick="Calculate('4');"/>
        <input type="button" id="btnFive" value="5" onclick="Calculate('5');"/>
        <input type="button" id="btnSix" value="6" onclick="Calculate('6');"/>
        <input type="button" id="btnNinus" value=" - " onclick="Calculate('-');"/>
        </td>
        </tr>
        <tr>
        <td>
       
        <input type="button" id="btnOne" value="1" onclick="Calculate('1');" />
        <input type="button" id="btnTwo" value="2" onclick="Calculate('2');"/>
        <input type="button" id="btnThree" value="3" onclick="Calculate('3');"/>
        <input type="button" id="btnDouble" value=" * " onclick="Calculate('*');"/>
        </td>
        </tr>
        <tr>
        <td>
         <input type="button" id="btnZero" value="0" onclick="Calculate('0');"/>
         <input type="button" id="btnDot" value="." onclick="Calculate('.');"/>
         <input type="button" id="btnClear" value="C" onclick="Clear();"/>
         <input type="button" id="btnDivide" value=" / " onclick="Calculate('/');"/>
        </td>
        </tr>
        <tr>
        <td colspan="3">
        <input type="button" id="btnResult" value=" = " onclick="Result();"/></td></tr>
        </table>
    </div>
    </form>
</body>
</html>