export async function getNotes(errorcallback) {
    let json;
    await fetch('api/notes')
        .then(response => json = response.json())
        .catch(error => {
            if (errorcallback)
                errorcallback(error)
        });
    
    return json;
}

export async function createNote(text, successcallback, errorcallback){
    await fetch('api/notes', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            text: text
        })
    }).then(response => {
        if (successcallback)
            successcallback(response)
    }).catch(error => {
        if (errorcallback)
            errorcallback(error)
    })
}
