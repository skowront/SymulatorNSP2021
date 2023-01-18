function saveFile(file, Content) {
    var link = document.createElement('a');
    link.download = file;
    console.log(link.download);
    link.href = "data:text/plain;charset=utf-8," + encodeURIComponent(Content)
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}