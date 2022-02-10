import DiscordJS, { Intents } from 'discord.js'
import dotenv from 'dotenv'
dotenv.config()
import { replies } from './replies'

const client = new DiscordJS.Client({
    intents: [
        Intents.FLAGS.GUILDS,
        Intents.FLAGS.GUILD_MESSAGES,
        Intents.FLAGS.GUILD_MEMBERS
    ]
})

// the number of messages (since the bot was run last). will need to store in DB if we want to keep it permanently
let numMessages = 0

// the prefix we are currently using, start off the message with the prefix so the bot knows that it needs to respond
const prefix = "!";

client.on('ready', () => {
    console.log('The bot is ready')

    // all of this stuff commented out is for slash commands if we want to implement them. not currently working, just the start. 

    // const guildId = '928010025958510632'
    // const guild = client.guilds.cache.get(guildId)
    // let commands

    // if (guild) {
    //     commands = guild.commands
    // } else {
    //     commands = client.application?.commands
    // }

    // commands?.create({
    //     name: 'ping',
    //     description: 'Replies with pong.',
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
            allMembersString += member.user.username;
            allMembersString += '\n';
        })
        const timeTaken = Date.now() - message.createdTimestamp;
        message.reply(`${allMembersString} \n(This message had a latency of ${timeTaken}ms.)`)

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


client.login(process.env.TOKEN)