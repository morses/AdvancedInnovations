export function replies(command: string | undefined) {
    if (command === "ping") {
        return "Pong";                
    }
    else if (command === "bing") {
        return "Bong";                
    } 
    else if (command === "ding") {
        return "Dong";                
    } 
    else if (command === "king") {
        return "Kong";
    }
    else if (command === "sing") {
        return "Song";
    }
}