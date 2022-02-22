import DiscordJS, { Intents, Presence } from 'discord.js'
import { MembershipStates } from 'discord.js/typings/enums';
import { dotenv } from 'dotenv'

$(document).ready(function () {
    dotenv.config()


    const AIServerID = 416485574631751690


    const client = new DiscordJS.Client({
        intents: [
            Intents.FLAGS.GUILDS,
            Intents.FLAGS.GUILD_MESSAGES,
            Intents.FLAGS.GUILD_MEMBERS,
            Intents.FLAGS.GUILD_PRESENCES
        ]
    })

    client.on('ready', () => {
        let allGuilds = "";
        client.guilds.cache.each((guild) => {
            allGuilds += guild.name + "," + guild.id + ";";
        })
    })
    client.login(process.env.TOKEN)

    $("#guilds").text(allGuilds)
});



