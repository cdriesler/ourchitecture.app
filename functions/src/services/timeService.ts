export function GetCurrentTime() {
    let t = new Date();

    let timestamp = t.getFullYear().toString() + "." + t.getUTCDate().toString() + t.getUTCMonth().toString() + "." + t.getUTCHours().toString() + t.getUTCMinutes().toString();

    return timestamp;
}