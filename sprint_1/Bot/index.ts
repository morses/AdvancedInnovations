process.env.NODE_TLS_REJECT_UNAUTHORIZED='0'
import  DiscordJS, { Intents, Presence } from 'discord.js'
import { MembershipStates } from 'discord.js/typings/enums';
import dotenv from 'dotenv'
dotenv.config()
import { replies } from './replies'




// const express = require('express');
// const app = express();
// const PORT = 8080;
// app.listen(
//     PORT,
//     () => console.log(`its alive on http://localhost:${PORT}`)
// )

const https = require('https')
const axios = require('axios')


// const ServerID = 928010025958510632


const client = new DiscordJS.Client({
    intents: [
        Intents.FLAGS.GUILDS,
        Intents.FLAGS.GUILD_MESSAGES,
        Intents.FLAGS.GUILD_MEMBERS,
        Intents.FLAGS.GUILD_PRESENCES
    ]
})

// the number of messages (since the bot was run last). will need to store in DB if we want to keep it permanently
let numMessages = 0;

// the prefix we are currently using, start off the message with the prefix so the bot knows that it needs to respond
const prefix = "!";

client.on('ready', () => {
    console.log('The bot is ready')

    // app.get('/guilds', (req: any, res: any) => {
    //     let allGuilds: string = "";
    //     client.guilds.cache.each((guild) => {
    //         allGuilds += guild.name + "," + guild.id + ";";
    //     })
    //     res.status(200).send({
    //         allGuilds
    //     })
    // })

    // app.get('/members', async (req: any, res: any) => {
    //     const list = client.guilds.cache.get(String(ServerID))
    //     const members = await list?.members.fetch();
    //     // var allMembersString = ""
    //     // members?.each((member) => {
    //     //     let presence;
    //     //     if (member.presence?.activities[0]?.name == undefined) {
    //     //         presence = "n/a"
    //     //     }
    //     //     else {
    //     //         presence = member.presence?.activities[0]?.name
    //     //     }
    //     //     allMembersString += member.user.username + "          Playing:  " + presence;
    //     //     allMembersString += '\n';
    //     // })
    //     res.status(200).send({
    //         members
    //     })
    // })
})


client.on('messageCreate', async(message) => {
    numMessages += 1;
    if (message.author.bot) return;
    if (!message.content.startsWith(prefix)) return;

    // gets the ID of the server in which the message was sent from
    const serverID = message.guild?.id;

    // gets the ID of the channel in which the message was sent from
    const channelID = message.channel.id;


    //removes the prefix and makes the command its own string
    const commandBody = message.content.slice(prefix.length);
    const args = commandBody.split(' ');
    const command = args.shift()?.toLowerCase();


    if(command === "members") {
        const list = client.guilds.cache.get(String(serverID))
        const members = await list?.members.fetch();
        var allMembersString = ""
        members?.each((member) => {
            let presence;
            if (member.presence?.activities[0]?.name == undefined) {
                presence = "n/a"
            }
            else {
                presence = member.presence?.activities[0]?.name
            }
            allMembersString += member.user.username + "          Playing:  " + presence;
            allMembersString += '\n';
        })
        const timeTaken = Date.now() - message.createdTimestamp;
        message.reply(`${allMembersString} \n(This message had a latency of ${timeTaken}ms.)`)

    }

    else if (command === "getweather") {

        axios.get('https://localhost:7228/api/guilds')
            .then((result: any) => {
                console.log(result)
                message.reply(result.data.toString());
            })
            .catch((error: any) => {
                // console.log(error);
            });
    }

    else if (command === "senduser") {
        axios.post('https://localhost:7228/api/guilds')
            .then((result: any) => {
                console.log(result)
                message.reply(result.data.toString());
            })
            .catch((error: any) => {
                console.log(error);
            });
    }

    else if (command === "testing") {
        let list = await client.guilds.cache.get(String(serverID))?.members.fetch()
        let users: any = []
        list?.each((user) => {
            if (user.user.bot === false) {
                let newUser = {
                    "Id": user.user.id.toString(),
                    "Name": user.user.username,
                    "Servers": "",
                    "Avatar": "null"
                }
                users.push(newUser)
            }
        })
        console.log(users)
        axios.post('https://localhost:7228/api/postusers', users)
            .then((result: any) => {
                console.log(result)
                message.reply(result.data.toString());
            })
            .catch((error: any) => {
                console.log(error);
            });
    }

    else if(command === "guilds") {
        let allGuilds = "";
        client.guilds.cache.each((guild) => {
            allGuilds += guild.name + '\n';
        })
        message.reply(allGuilds)
    }

    else if (command === "total"){
        const timeTaken = Date.now() - message.createdTimestamp;
        message.reply(`${numMessages} \n(This message had a latency of ${timeTaken}ms.)`)
    }

    else {
        const timeTaken = Date.now() - message.createdTimestamp;
        message.reply(`${replies(command)} \n(This message had a latency of ${timeTaken}ms.)`);
    }
})



async function sendUsers (serverId: string){
    let list = await client.guilds.cache.get(String(serverId))?.members.fetch()
        let users: any = []
        list?.each((user) => {
            if (user.user.bot === false) {
                if (user.user.avatar === null) {
                    user.user.avatar = "null";
                }
                let newUser = {
                    "Id": user.user.id.toString(),
                    "Name": user.user.username,
                    "Servers": serverId,
                    "Avatar": user.user.avatar
                }
                users.push(newUser);
            }
        })
        console.log(users)
        axios.post('https://localhost:7228/api/postusers', users)
            .then((result: any) => {
                console.log(result);
            })
            .catch((error: any) => {
                console.log(error);
            });
}

function updataData() {
    client.guilds.cache.each((guild) => {
        sendUsers(guild.id)
    })
}

setInterval(updataData, 10000)


client.login(process.env.TOKEN)