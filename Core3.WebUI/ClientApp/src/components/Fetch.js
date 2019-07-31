export async function getNotes() {
    const response = await fetch('api/notes');
    console.log(response);
    const json = await response.json();
    console.log(json);
    return json;
}