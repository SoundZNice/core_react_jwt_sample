export async function getNotes() {
    const response = await fetch('api/notes');

    return {
        obj: response.json(),
        status: response.status
    };
}

export async function createNote(text) {
    const response = await fetch('api/notes', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            text: text
        })
    });

    return response.status;
}


