function typeInTextarea(newText, el) {
    const [start, end] = [el.selectionStart, el.selectionEnd];
    el.setRangeText(newText, start, end, 'select');
}

function formatText(tag) {
    var Field = document.getElementById('post');
    var val = Field.value;
    var selected_txt = val.substring(Field.selectionStart, Field.selectionEnd);
    var before_txt = val.substring(0, Field.selectionStart);
    var after_txt = val.substring(Field.selectionEnd, val.length);
    typeInTextarea('[' + tag + ']' + '[/' + tag + ']', Field)
}