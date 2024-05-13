var regexEmailAddress = /[!#-9A-~]+@+[a-z0-9]+.+[^.]$/;
function validateEmail(val) {
        var exp = new RegExp(regexEmailAddress);
    exp.Multiline = true;
    return exp.test(val);
}