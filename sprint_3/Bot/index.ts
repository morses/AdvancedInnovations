process.env.NODE_TLS_REJECT_UNAUTHORIZED='0'
import  DiscordJS, { Guild, Intents, Presence } from 'discord.js'
import { MembershipStates } from 'discord.js/typings/enums';
import dotenv from 'dotenv'
dotenv.config()
import { replies } from './replies'


// const url = 'https://discordstats.azurewebsites.net'
const url = 'https://localhost:7228'


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
        Intents.FLAGS.GUILD_PRESENCES,
        Intents.FLAGS.GUILD_VOICE_STATES
    ]
})

// the number of messages (since the bot was run last). will need to store in DB if we want to keep it permanently
let numMessages = 0;

// the prefix we are currently using, start off the message with the prefix so the bot knows that it needs to respond
const prefix = "!";

client.on('ready', () => {
    console.log('The bot is ready')
})


client.on('messageCreate', async(message) => {
    const serverID = message.guild?.id;
    const channelID = message.channel.id;

    if (message.author.bot) return;

    let MessageInfo = {
        serverId : serverID,
        channelId : channelID,
        userId : message.author.id,
        createdAt : message.createdAt
    }
    setTimeout(() => {
        axios.post(url + '/api/PostMessageData', MessageInfo)
            .then((result: any) => {
                console.log(result);
            })
            .catch((error: any) => {
                console.log(error);
            });
    }, 5000);








    if (!message.content.startsWith(prefix)) return;

    // gets the ID of the server in which the message was sent from

    // gets the ID of the channel in which the message was sent from

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

        axios.get(url + '/api/guilds')
            .then((result: any) => {
                console.log(result)
                message.reply(result.data.toString());
            })
            .catch((error: any) => {
                // console.log(error);
            });
    }

    else if (command === "senduser") {
        axios.post(url + '/api/guilds')
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
        axios.post(url + '/api/postusers', users)

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



async function sendUsers (){
    let users: any = []
    client.guilds.cache.each(async (guild) => {
        let list = await client.guilds.cache.get(String(guild.id))?.members.fetch()
        list?.each((user) => {
            if (user.user.bot === false) {
                if (user.user.avatar === null) {
                    user.user.avatar = "null";
                }
                let newUser = {
                    "Id": user.user.id.toString(),
                    "Username": user.user.username,
                    "Servers": guild.id,
                    "Avatar": user.user.avatar
                }
                // console.log(newUser)
                users.push(newUser);
                setTimeout(() => 100);
            }
            // console.log(users.length)
        })


    })
    setTimeout(() => {
        console.log("The users of all servers: ")
        console.log(users)
        axios.post(url + '/api/postusers', users)

            .then((result: any) => {
                console.log(result);
            })
            .catch((error: any) => {
                console.log(error);
            });
    }, 5000);
}



async function sendServers (){
    let servers: any = []
    client.guilds.cache.each(async (guild) => {
        const list = client.guilds.cache.get(String(guild.id))
        const members = await list?.members.fetch();


        let presenceCount = 0;
        members?.forEach((member) => {
            if (member.presence != undefined && member.presence != null && member.presence.activities.length != 0 && member.user.bot === false) {
                presenceCount += 1
            };
        });

        let server = {
            ID: guild.id,
            Name: guild.name,
            Owner: "false",
            Icon: guild.icon,
            HasBot: "true",
            ApproximateMemberCount: members?.size,
            OwnerId: guild.ownerId,
            VerificationLevel: guild.verificationLevel,
            Description: guild.description,
            Premiumtier: guild.premiumTier,
            ApproximatePresenceCount: presenceCount,
            Privacy: "null",
            OnForum: "null",
            Message: "null"
        }
        servers.push(server);
    })
    setTimeout(() => {
        if (servers.length != 0) {
            axios.post(url + '/api/postservers', servers)
                .then((result: any) => {
                    console.log(result);
                })
                .catch((error: any) => {
                    console.log(error);
                });
        }
    }, 5000);
}


async function sendChannels (){
    let channels: any = []
    client.guilds.cache.each(async (guild) => {
        guild.channels.cache.each(async (channel) => {
            let newChannel = {
                Id: channel.id,
                Type: channel.type,
                Name: channel.name,
                Count: null,
                GuildId: guild.id
            }
            channels.push(newChannel);
        })
    })
    setTimeout(() => {
        if (channels.length != 0) {
            console.log("All Channels: ")
            console.log(channels)
            axios.post(url + '/api/postchannels', channels)

                .then((result: any) => {
                    console.log(result);
                })
                .catch((error: any) => {
                    console.log(error);
                });

            //axios.post('https://discordstats.azurewebsites.net/channel/postchannels', channels)
            //    .then((result: any) => {
            //        console.log(result);
            //    })
            //    .catch((error: any) => {
            //        console.log(error);
            //    });
        }
    }, 5000);
}

async function sendPresence (){
    let presences: any = []

    client.guilds.cache.each(async (guild) => {

        const list = client.guilds.cache.get(String(guild.id))
        const members = await list?.members.fetch();
    
    
        members?.forEach((member) => {
            if (member.presence === undefined || member.presence === null) {
                return;
            };
    
            if (member.presence.activities.length === 0) {
                return;
            };
            let details = member.presence?.activities[0].details
            if (member.presence?.activities[0].details == null) {
                details = "null";
            }
    
            if (member.user.bot === false) {
                let newPresence = {
                    "Id": member.presence.activities[0].id,
                    "ApplicationId": member.presence?.activities[0].applicationId,
                    "Name": member.presence?.activities[0].name,
                    "Details": details,
                    "CreatedAt": member.presence?.activities[0].createdAt,
                    "LargeImageId": member.presence?.activities[0].assets?.largeImage,
                    "SmallImageId": member.presence?.activities[0].assets?.smallImage,
                    "Image": member.presence?.activities[0].assets?.largeImageURL(),
                    "ServerId": guild.id,
                    "UserId": member.id
                };
                presences.push(newPresence);
            };
            
        })
    
    });

    setTimeout(() => {
        if (presences.length > 0) {
            console.log("The presence of all users: ")
            console.log(presences)
            axios.post(url + '/api/postpresence', presences)

            .then((result: any) => {
                console.log(result);
            })
            .catch((error: any) => {
                console.log(error);
            });
        }
    }, 5000);
};


function guildIdAndAllUsersId(){
    let users: any = []
    client.guilds.cache.each(async (guild) => {
        let list = await client.guilds.cache.get(String(guild.id))?.members.fetch()
        list?.each((user) => {
            if (user.user.bot === false) {
                if (user.user.avatar === null) {
                    user.user.avatar = "null";
                }
                let newUser = {
                    "Id": user.user.id.toString(),
                    "Username": user.user.username,
                    "Servers": guild.id,
                    "Avatar": user.user.avatar
                }
                // console.log(newUser)
                users.push(newUser);
                setTimeout(() => 100);
            }
        })
    })

    client.guilds.cache.each(async (guild) => {
        let list = client.guilds.cache.get(String(guild.id))?.channels.cache.forEach(ch => {
            if (ch.type === "GUILD_TEXT"){
                ch.messages.fetch({
                    limit: 100
                }).then(messages => {
                    const msgs = messages.filter(m => m.author.id === users)
                    msgs.forEach(m => {
                        console.log(`${m.content} - ${m.channel}`)
                        console.log('hello')
                    })
                })
            } else {
                return;
            }
        })
    })
};
async function sendVoiceChannels (){
    let channels: any = []

     client.guilds.cache.each(async (guild) => {
        guild.channels.cache.each(async (channel) => {               
            
            if(channel.type == 'GUILD_VOICE' && channel.members.size != 0)
            {
            let newChannel = {
                Id: channel.id, 
                Name: channel.name, 
                Count: channel.members.size, 
                GuildId: guild.id
            }           
            channels.push(newChannel);
            }
        }) 
       })

    setTimeout(() => {
        if (channels.length != 0) {
            console.log("All Channels: ")
            console.log(channels)
        
            axios.post('https://localhost:7228/api/PostVoiceChannels', channels)
                .then((result: any) => {
                    console.log(result);
                })
                .catch((error: any) => {
                    console.log(error);
                });
        }
    }, 100000);
}

            // axios.post('https://discordstats.azurewebsites.net/api/postchannels', channels)
            //     .then((result: any) => {
            //         console.log(result);
            //     })
            //     .catch((error: any) => {
            //         console.log(error);
            //     });


// async function userMessages(guildID, userID){
//     client.guilds.cache.get(guildID).channels.cache.forEach(ch => {
//         if (ch.type === 'text'){
//             ch.messages.fetch({
//                 limit: 100
//             }).then(messages => {
//                 const msgs = messages.filter(m => m.author.id === userID)
//                 msgs.forEach(m => {
//                     console.log(`${m.content} - ${m.channel.name}`)
//                 })
//             })
//         } else {
//             return;
//         }
//     })
// }

function updataData() {
    sendPresence();
    sendUsers();
}
function UpdateVoiceChannel() {
    sendVoiceChannels();
    sendServers();
    sendChannels();
}
  
setInterval(updataData, 12000);
setInterval(UpdateVoiceChannel, 1800000);

client.login(process.env.TOKEN);