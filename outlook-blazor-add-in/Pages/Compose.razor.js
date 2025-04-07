export async function getEmailData(IncludeFullConversation) {

    try {
        console.log(`Index.razor.js(getEmailData) Reading mailbox item`);
        const result = await new Promise((resolve, reject) => {
            Office.context.mailbox.item.body.getAsync("html", function (result) {
                if (result.status === Office.AsyncResultStatus.Succeeded) {
                    resolve(result.value);
                } else {
                    reject(result.error.message);
                }
            });
        });
        var emailBody = html2text(result, IncludeFullConversation);
        var userName = Office.context.mailbox.userProfile.displayName;
        var data = {
            EmailBody: emailBody,
            UserName: userName
        };
        return JSON.stringify(data);

    } catch (error) {
        console.error(`Index.razor.js(getEmailData) Catch Exception: ${err}`);
        subject = `${err}`;
        return { Subject: subject };
    }
}

export function html2text(html, IncludeFullConversation) {
    var parts = html;
    if (IncludeFullConversation == false) {
        parts = html.split('From:')[0].trim();
    }

    var div = document.createElement("div");

    div.innerHTML = parts;
    var text = div.textContent || div.innerText || "";
    text = text.trim();
    text = text.replace(/<!--[\s\S]*?-->/g, "");
    return text;
}


export async function replyAll(chatGPTResponse) {

    if (chatGPTResponse.length >= 2 && chatGPTResponse.substring(0, 1) === "\n") {
        chatGPTResponse = chatGPTResponse.substring(1);
    }
    chatGPTResponse = chatGPTResponse.replace(/(?:\r\n|\r|\n)/g, '<br>');
    Office.context.mailbox.item.displayReplyAllForm(
    {
            'htmlBody': chatGPTResponse
    });
}
