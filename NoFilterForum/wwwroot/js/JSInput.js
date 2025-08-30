function addBoldText(){
    let textInput = document.getElementById('textInput');
    textInput.value+='<b>BOLD TEXT</b>';
}function addItalicText(){
    let textInput = document.getElementById('textInput');
    textInput.value+='<i>ITALIC TEXT</i>';
}function addBulletPoint(){
    let textInput = document.getElementById('textInput');
    textInput.value+='&bull;';
}
function addSmallerText(){
    
    let textInput = document.getElementById('textInput');
    textInput.value+='<span class="smallerText">SMALLER TEXT</span>';
}
function addBiggerText(){
    
    let textInput = document.getElementById('textInput');
    textInput.value+='<span class="biggerText">BIGGER TEXT</span>';
}