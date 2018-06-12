///
function ConvertDateTimeSystem(dtinput)
{
    if (dtinput == "")
        return dtinput;
    var res = dtinput.replace(/-/g, "/");
    var arrdt = dtinput.split('/');
    return arrdt[2] + "-" + arrdt[1] + "-" + arrdt[0];
}