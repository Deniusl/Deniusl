mergeInto(LibraryManager.library, {
  CopyToClipboard: function(text) {
    var textString = UTF8ToString(text);
    console.log("Attempting to copy: " + textString);
    
    navigator.permissions.query({name: "clipboard-write"}).then(function(result) {
      if (result.state === "granted" || result.state === "prompt") {
        navigator.clipboard.writeText(textString).then(function() {
          console.log('Copying to clipboard was successful!');
        }, function(err) {
          console.error('Could not copy text: ', err);
        });
      } else {
        console.error('Clipboard permission denied');
      }
    });
  }
});