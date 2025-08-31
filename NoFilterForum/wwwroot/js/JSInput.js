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
function addGreenText(){
    
    let textInput = document.getElementById('textInput');
    textInput.value+='<span class="greenText">GREEN TEXT</span>';
}
function addRedText(){
    
    let textInput = document.getElementById('textInput');
    textInput.value+='<span class="redText">RED TEXT</span>';
}
function addBlueText(){
    
    let textInput = document.getElementById('textInput');
    textInput.value+='<span class="blueText">BLUE TEXT</span>';
}
document.addEventListener('DOMContentLoaded',(e)=>{
let colorWheel=document.getElementById('colorWheel');
colorWheel.addEventListener('change',(e)=>{
    let textInput = document.getElementById('textInput');
    textInput.value+=`<span style='color:${colorWheel.value}'>COLOR TEXT</span>`;
});
});