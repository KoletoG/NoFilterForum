function addBoldText(){
    let textInput = document.getElementById('textInput');
    textInput.innerHTML+='<b>BOLD TEXT</b>';
}function addItalicText(){
    let textInput = document.getElementById('textInput');
    textInput.innerHTML+='<i>ITALIC TEXT</i>';
}function addBulletPoint(){
    let textInput = document.getElementById('textInput');
    textInput.innerHTML+='&bull;';
}
function addSmallerText(){
    
    let textInput = document.getElementById('textInput');
    textInput.innerHTML+='<span class="smallerText">SMALLER TEXT</span>';
}
function addBiggerText(){
    
    let textInput = document.getElementById('textInput');
    textInput.innerHTML+='<span class="biggerText">BIGGER TEXT</span>';
}