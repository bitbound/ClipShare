window.getClipboard = async () => {
    var contents = await navigator.clipboard.readText();
    return contents;
}