﻿using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using SmbiBotApp.Services;
using Microsoft.Bot.Builder.Luis.Models;
using SmbiBotApp.Model;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Text;
using Microsoft.Bot.Builder.Dialogs;
using System.Reflection;
using Autofac;
using System.Collections.Generic;
using System.Windows.Input;
using System.Web.UI.WebControls;
using Facebook;
using System.Threading;
using Humanizer;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Xml;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;


namespace SmbiBotApp
{

    [Serializable]
    //public class DisplayDialog : IDialog<object>
    //{
    //    private ResumeAfter<bool> play;

    //    public  async Task StartAsync(IDialogContext context)
    //    {
    //       await context.PostAsync("Are you sure you want to save");
    //       // PromptDialog.Confirm(context,play,"create the profile");
    //      //  await context.Wait(SendAsync);
    //          context.Wait(MessageReceivedAsync);       
    //    }

    //    public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    //    {
    //        await context.PostAsync("Not correct. Guess again.");
    //        PromptDialog.Confirm(context,play,"Yes I did it");
            
    //        // context.Wait(MessageReceivedAsync);          
    //    }
    //}

    [BotAuthentication]
    public class MessagesController : ApiController
    {

        public List<string> col = new List<string>();
        public static List<string> data = new List<string>();
        public static List<string> avoid = new List<string>();
        static string[] profile = new string[7];
        static int count = 0;
        static int flag = 0;
        static int counter = 1;
        static int callback = 0;
        static string id = null;
        static string occupation = null;
        static int pro_count = 1;
        static bool wait = true;
        static int ques_count = 0;
        static int score = 0;
        static int flagi = 0;
        public static int repeat = 1;
        List<string> ques = new List<string>();
        List<string> options = new List<string>();
        public static Testing testsretrieved = null;
        public static Record[] display = null;


        public enum decision
        {
            firstName = 1,
            lastName = 2,
            myAge = 3,
            Gender = 4,
            currentAddress = 5,
            emailAddress = 6,
            university = 7,
            graduate = 8,
            degree = 9,
            projects = 10,
            title = 11,
            description = 12,

        };
        private void asked()
        {
            col.Add("Before I get matching you to the perfect job. I'll have to first setup your profile.");
            col.Add("This is the information we were able to pull from facebook could you confirm if you would like me to use this information ?");
            col.Add("What is your firstname ?");
            col.Add("What is your last name ?");
            col.Add("How old are you ?");
            col.Add("Are you a guy or a girl ?");
            col.Add("Where are you currently living ?");
            col.Add("what is your email address ?");
            col.Add("Now that we're acquainted I would like to learn a little about your education and skills.");
            col.Add("What university did you attend ?");//9
            col.Add("When did you graduate ?");//10
            col.Add("What did you study ?");//11
            col.Add("Is this the highest level of education you attainted ?");//12
            col.Add("What sort of company are you interested in working for:");//13
            col.Add("What sort of designer are you ?");//14
            col.Add("How many projects or products you have worked on ?");//15
            col.Add("what is the title of your");//16
            col.Add("Briefly describe your"); //17
            col.Add("Rank your experience / level of using design softwares:Sketch:Photoshop:Illustrator:indesign:");
            col.Add("What web development languages are you familiar with : (select all those that apply)");//19
            col.Add("Describe the project or role you were most proud of ? What sort did you do in your capcity and what were the outcomes / achievements ? ");
            col.Add("Is there any other project, product , or work experience. You would like to share with us ?");
            col.Add("what sort of developer are you ?");//22
            col.Add("my first name is");//13//23
            col.Add("my last name is");//14//24
            col.Add("my age is");//15//25
            col.Add("my gender is");//16//26
            col.Add("I currently live in");//17//26//27
            col.Add("my email address is");//28
            col.Add("I attend");//29
            col.Add("I graduated in");//30
            col.Add("I study");//31
            col.Add("Are you done");//32
            col.Add("What sort of role are you looking for ?");//33
            col.Add("Your profile has been successfully created");//34
            col.Add(" I'm maaz a chabot developed by folks at PeopleHome to help you find the job right for you !");//35
            col.Add("Project that i have been done so far are");//36
            col.Add("The title of my project is");//37
            col.Add("The description of my project is");//38
            col.Add("That's good now your basic profile has been set.If you want to update your profile information let us know ?");//39
            col.Add("Now we are going to take some tests to evaluate you and to build your professional profile");//40
            col.Add("Which technology you are intertested ?");//41
            col.Add("Which type of test you would like to give ?"); //42
            col.Add("Now let start the test here is your first question");//43
            col.Add("Do you want to give any other test ?");//44
            col.Add("Ok thanks for visiting our bot and share your experience");//45
            col.Add("You have been already given that test");//46
            col.Add("Which information would you like to update ?");//39


            ques.Add("HTML is what type of language");
            ques.Add("HTML use");
            ques.Add("The year in which HTML was first proposed _______.");

            options.Add("Scripting Language,Markup Language,Programming Language,Network Protocol");
            options.Add("User defined tags, Pre - specified tags, Fixed tags defined by the language, Tags only for linking");
            options.Add("1990,1980,2001,990");



        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (wait == true)
            {
                wait = false;
                asked();
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await HandleSystemMessage(activity, connector);
                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                wait = true;
                return response;

            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }

        }

        private async Task<Activity> HandleSystemMessage(Activity activity, ConnectorClient connector)
        {
            var replymesge = string.Empty;
            Activity reply = new Activity();
            reply = activity.CreateReply();
            if (activity.Type == ActivityTypes.Message)
            {
                var phrase = activity.Text;
             
                //XmlDocument doc = new XmlDocument();
                //doc.Load("C:\\git\\BotRepository\\Bot Application3\\files\\usanew.xml");
                //string jsonText = JsonConvert.SerializeXmlNode(doc); //XML to Json
                //          // string json = JsonConvert.SerializeObject(jsonText);

                ////write string to file
                ////  System.IO.File.WriteAllText("C:\\test\\path.json", jsonText);
                //var bson = BsonSerializer.Deserialize<BsonDocument>(jsonText); //Deserialize JSON String to BSon Document

                //var Client = new MongoClient();
                //var MongoDB = Client.GetDatabase("test");
                //var Collec = MongoDB.GetCollection<BsonDocument>("questions");

                ////var pro = new BsonDocument
                ////  {
                ////      {"totalquest" , new BsonArray().Add(bson) }
                // // };
                //await Collec.InsertOneAsync(bson);

                //   //  var mcollection = Program._database.GetCollection<BsonDocument>("test_collection_05");
                //   // await mcollection.InsertOneAsync(bsdocument); //Insert into mongoDB


                //   // Insert new user object to collection
                //   //users.Insert(user);
                ////   LuisService.DoSomethingAsync(jsonText);
                //   //LuisService.DoSomethingAsync();



                var luisresp = await LuisService.ParseUserInput(phrase);

             back:

                if (luisresp.intents.Count() > 0)
                {

                    var str = luisresp.topScoringIntent;
                    try
                    {
                        var symb = string.Empty;
                    next:
                        switch (str.intent)
                        {
                            case "None":
                                callback = 1;
                                switch (count)
                                {
                                    case 1:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(23) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 2:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(24) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 3:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(25) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 4:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(26) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 5:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(27) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 6:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(28) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 7:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(29) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 8:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(30) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 9:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(31) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 10:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(36) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 11:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(37) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;

                                    case 12:
                                        luisresp = await LuisService.ParseUserInput(col.ElementAt(38) + " " + luisresp.query);
                                        replymesge = luisresp.query;
                                        break;
                                }

                                break;

                            case "questions":
                                callback = 0;
                                replymesge = luisresp.query;
                                var entity = luisresp.entities[0].entity.ToLower();
                                if (!avoid.Contains(entity))
                                {
                                    repeat = 1;
                                    avoid.Add(entity);
                                    if (entity.Substring(0, 4) == "mode")
                                    {
                                        entity = entity.Substring(0, 4);
                                    }
                                    else if (avoid.Contains("thanks"))
                                    {
                                        avoid.Add("another");
                                    }
                                    else if (avoid.Contains("test"))
                                    {
                                        avoid.Remove("test");
                                    }
                                    switch (entity)
                                    {
                                        case "started":

                                            FacebookServices fbc = new FacebookServices();
                                            profile = await fbc.GetUser("EAABdYDHKoG8BAJ0FkrKPTfCoJmHSEIyVkmLn6iXTPIxU8KRXIZCx5sQEJMSD0APTBz3vQI3CXalw0ZCPKZAyjZBbcjKeZB75arA2ZC1F7Jczfx0bKqKzDBRpZA1eFGJZAvsJvsx1zLA51JBw2vNhuJhDkonMp68zG6oZD");
                                            if (profile != null)
                                            {
                                                id = profile[0];
                                                data.Add(id);
                                                var result = MongoUser.exist_user(profile[0]);
                                                string location = profile[5];
                                                string[] loc = (location.Substring(location.IndexOf(location.Substring(32))).Split(','));
                                                profile[5] = loc[0];
                                                if (result != null)
                                                {
                                                    var res = JsonConvert.DeserializeObject<MongoData>(result.ToJson());
                                                    if (res.basic.status == 1)
                                                    {
                                                        count = 6;
                                                        counter = 6;
                                                        str.intent = "emailAddress";
                                                        goto next;
                                                    }
                                                    else if (res.basic.status == 2)
                                                    {
                                                        count = 10;
                                                        counter = 10;
                                                        str.intent = "projects";
                                                        goto next;
                                                    }
                                                    else if (res.basic.status == 3)
                                                    {
                                                        count = 12;
                                                        counter = 12;
                                                        str.intent = "test";
                                                        goto next;
                                                    }
                                                }
                                                else
                                                {
                                                    reply.Text = "Welcome" + " " + profile[1] + " !" + col.ElementAt(35);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    Thread.Sleep(1000);
                                                    replymesge = col.ElementAt(33);
                                                    JobOptions(reply);
                                                }
                                            }

                                            break;
                                        case "position":
                                            JobOptions(reply);
                                            break;
                                        case "profile":
                                            string[] occup = luisresp.query.Split();
                                            occupation = occup[0];
                                            reply.Text = col.ElementAt(0);
                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                            Thread.Sleep(1000);
                                            reply.Text = "First Name:" + profile[1] + "\n\n" + "Last Name:" + profile[2] +
                                                         "\n\n" + "Gender:" + profile[4] + "\n\n" + "Location:" + profile[5] + "\n\n" + "Email:" + profile[6];
                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                            Thread.Sleep(1100);
                                            replymesge = col.ElementAt(1);
                                            infoConfirm(reply);
                                            break;
                                        case "basic":
                                            count = 7;
                                            counter = 7;
                                            data.Clear();
                                            for (int i = 0; i <= 6; i++)
                                            {
                                                data.Add(profile[i]);
                                            }
                                            check_status(1);

                                            reply.Text = col.ElementAt(8);
                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                            Thread.Sleep(2000);
                                            replymesge = col.ElementAt(9);

                                            break;
                                        case "details":
                                            replymesge = col.ElementAt(2);
                                            count = 1;
                                            break;
                                        case "company":
                                            replymesge = col.ElementAt(13);
                                            selectCompany(reply);
                                            break;

                                        case "interested":
                                            replymesge = refferedtochoice(id, reply);
                                            data.Add(luisresp.query.Substring(25));
                                            break;

                                        case "designer":
                                            string ski = luisresp.query.Substring(23);
                                            ski = string.Concat((ski).Split('/')).Replace(" ", "");
                                            var qury = MongoUser.get_skill_id(ski).ToList();
                                            var value = JObject.Parse(qury[1].Value.ToString())[ski].Value<string>();
                                            data.Add(value);
                                            replymesge = col.ElementAt(15);
                                            break;

                                        case "attend":
                                            count = 7;
                                            counter = 7;
                                            replymesge = col.ElementAt(9);
                                            break;

                                        case "update":
                                            replymesge = col.ElementAt(37);
                                            update_profile(reply);
                                            break;

                                        case "test":

                                            var usertest = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                                            if (usertest.test != null)
                                            {
                                                var check = usertest.test.Where(x => x.technology.ToLower() == activity.Text.Substring(22).ToLower()).ToArray();
                                                if (check.Count() != 0)
                                                {
                                                    replymesge = col.ElementAt(46);
                                                    flagi = 0;
                                                }
                                                else
                                                {
                                                    data.Add(activity.Text.Substring(22).ToLower());
                                                    flagi = 2;
                                                }
                                            }
                                            else
                                            {
                                                data.Add(activity.Text.Substring(22).ToLower());
                                                flagi = 1;
                                            }
                                            if (flagi != 0)
                                            {

                                                reply.Text = col.ElementAt(43);
                                                await connector.Conversations.ReplyToActivityAsync(reply);
                                                Thread.Sleep(2000);

                                                testsretrieved = JsonConvert.DeserializeObject<Testing>(MongoUser.ret_sel_test(activity.Text.Substring(22).ToLower()).ToJson());
                                                display = (testsretrieved.record.Where(x => x.type.ToLower() == activity.Text.Substring(22).ToLower())).ToArray();
                                                replymesge = display[0].Statements;
                                                test(reply, display[0].Options, 0);

                                            }


                                            break;

                                        case "another":
                                            replymesge = col.ElementAt(41);
                                            choose_tech(reply);
                                            break;

                                        case "thanks":
                                            replymesge = col.ElementAt(45);
                                            commands(reply);
                                            break;

                                        case "data":
                                            break;

                                        case "scores":
                                            break;

                                        case "mode":

                                            if (ques_count <= display.Count())
                                            {
                                                var ans = activity.Text.Substring(0, 1);
                                                if (display[ques_count].Answers == ans)
                                                {
                                                    score++;
                                                }
                                                ++ques_count;
                                                if (ques_count < display.Count())
                                                {
                                                    replymesge = display[ques_count].Statements;
                                                    test(reply, display[ques_count].Options, ques_count);
                                                    Thread.Sleep(1100);
                                                }
                                                else
                                                {
                                                    data.Add(score.ToString());
                                                    reply.Text = "You have completed the" + data[1] + "track.Your results are as follows";
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    reply.Text = "Technology:" + " " + data[1] + "\n\n" + "Score:" + " " + score;
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    Thread.Sleep(1000);
                                                    save_user_test(flagi);
                                                    replymesge = col.ElementAt(44);
                                                    test_again(reply);

                                                }
                                            }

                                            break;
                                    }
                                }
                                else
                                {
                                    repeat = 0;
                                }
                                break;

                            default:
                                callback = 0;
                                switch (counter)
                                {
                                    case 1:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 3);
                                        break;

                                    case 2:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 4);
                                        break;

                                    case 3:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 5);
                                        break;
                                    case 4:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 6);
                                        break;

                                    case 5:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 7);
                                        break;

                                    case 6:

                                        if (counter == 6 && data.Count == 6)
                                        {
                                            check_entity(symb, luisresp);
                                            if (data.Count == 7)
                                            {
                                                check_status(0);
                                            }
                                        }
                                        reply.Text = correctSequence(str.intent, replymesge, 8);
                                        await connector.Conversations.ReplyToActivityAsync(reply);
                                        Thread.Sleep(2000);
                                        replymesge = col.ElementAt(9);
                                        break;

                                    case 7:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 10);
                                        break;

                                    case 8:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 11);
                                        break;

                                    case 9:
                                        check_entity(symb, luisresp);
                                        replymesge = correctSequence(str.intent, replymesge, 12);
                                        yesorno(reply);
                                        break;

                                    case 10: 
                                        if (counter == 10 && data.Count >= 6)   
                                        {
                                            check_entity(symb, luisresp);
                                            edu_pro_record();

                                        }

                                      
                                        var number = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                                        if (pro_count > 0 && pro_count <= number.project.no_of_projects)
                                        {
                                            if (pro_count > 1)
                                            {
                                                data.Add(luisresp.query.Substring(41));
                                            }

                                            replymesge = correctSequence("projects", replymesge, 16) + " " + pro_count.ToOrdinalWords() + " project ?";

                                        }
                                        else
                                        {
                                            data.Add(luisresp.query.Substring(41));
                                            pro_details();
                                            reply.Text = col.ElementAt(39);
                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                            Thread.Sleep(2000);
                                            reply.Text = col.ElementAt(40);
                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                            Thread.Sleep(2000);
                                            replymesge = col.ElementAt(41);
                                            choose_tech(reply);
                                            count = count + 2;
                                            counter = counter + 2;
                                        }
                                        break;

                                    case 11:
                                        var numb = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                                        if (pro_count > 0 && pro_count <= numb.project.no_of_projects)
                                        {

                                            if (pro_count == 1)
                                                data.Add(luisresp.query);
                                            else
                                                data.Add(luisresp.query.Substring(27));

                                            
                                            replymesge = correctSequence("title", replymesge, 17) + " " + pro_count.ToOrdinalWords() + " project";
                                            pro_count++;
                                            count = count - 2;
                                            counter = counter - 2;
                                        }
                                        else
                                        {

                                        }
                                        break;

                                    case 12:
                                        replymesge = col.ElementAt(41);
                                        choose_tech(reply);
                                        break;
                                }
                                break;

                        }
                        if (callback == 1)
                        {
                            callback = 0;
                            goto back;
                        }
                        if (symb != string.Empty)
                        {
                            data.Add(symb);
                        }

                        phrase.ToLower();
                        if (flag == 1 && (phrase == "yes" || phrase == "y"))
                        {
                            if (data.Count == 4)
                            {
                            }
                        }
                    }
                    catch (FacebookApiException ex)
                    {
                        replymesge = ex.Message.ToString();
                    }
                    if (repeat == 1)
                    {
                        reply.Text = replymesge;
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                    
                }

                else
                {
                    replymesge = $"sorry.I could not get as to what you say";
                }
            }

            else if (activity.Type == ActivityTypes.DeleteUserData)
            {

                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                count = 1;
                counter = 1;
                data.Clear();

                //IConversationUpdateActivity update = activity;
                //using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                //{
                //    //data.Clear();
                //    var client = scope.Resolve<IConnectorClient>();
                //    if (update.MembersAdded.Any())
                //    {
                //        var repli = activity.CreateReply();
                //        //repli.Attachments = new List<Attachment>();
                //        var newMembers = update.MembersAdded?.Where(t => t.Id != activity.Recipient.Id);

                //        //List<CardAction> cardButtons = new List<CardAction>();
                //        //CardAction plButton = new CardAction()
                //        //{
                //        //    Value = "what sort of position are you looking for ?",
                //        //    Type = "postBack",
                //        //    Title = "Get Started"
                //        //};
                //        //cardButtons.Add(plButton);
                //        ////  JobOptions(repli);
                //        //HeroCard plCard = new HeroCard()
                //        //{
                //        //    Buttons = cardButtons
                //        //};
                //        //Attachment plAttachment = plCard.ToAttachment();
                //        //repli.Attachments.Add(plAttachment);

                //        foreach (var newMember in newMembers)
                //        {
                //            repli.Text = "Welcome";
                //            if (!string.IsNullOrEmpty(newMember.Name))
                //            {
                //                repli.Text += $" {newMember.Name}";
                //            }
                //            repli.Text += "! I'm Maz created by the people at PeopleHome to help you find the job right for you. So let's get-started ?";
                //            repli.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                //            await connector.Conversations.ReplyToActivityAsync(repli);
                //        }
                //    }
                //}

                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

       private async Task<Tuple<LuisResponse, string>> CallLuisAgain(LuisResponse luisresp, string reply, int y)
        {
            luisresp = await LuisService.ParseUserInput(col.ElementAt(y) + " " + luisresp.query);
            reply = luisresp.query;
            var tuple = new Tuple<LuisResponse, string>(luisresp, reply);
            return tuple;
        }

        private void check_entity(string symb, LuisResponse luisresp)
        {
            symb = luisresp.entities[0].entity;
            if (symb != string.Empty)
            {
                data.Add(symb);
            }
        }

        private void save_user_test(int flagi)
        {
            int j = 1;
            var record = new BsonDocument
            {
                { "technology" , data.ElementAt(j++) },
                { "score"      , data.ElementAt(j++) },
            };

            MongoUser.upd_test_info(record, flagi, id);
            data.Clear();
            data.Add(id);
        }



        private void pro_details()
        {

            int cou = data.Count;//3 // 5// 7
            cou = (cou - 1) / 2; // 3 -1 = 2 // 5-1 = 4 // 7-1 = 6

            int j = 1;
            List<BsonDocument> multiple = new List<BsonDocument>();
            for (int i = 0; i < cou; i++)
            {
                var record = new BsonDocument
                        {
                           { "title"    , data.ElementAt(j++) },
                           { "description"   , data.ElementAt(j++) },
                        };

                multiple.Add(record);
            }

            MongoUser.upd_project_info(multiple, 3, id);
            data.Clear();
            data.Add(id);
        }



        private void edu_pro_record()
        {

            int cou = data.Count;//10 // 7// 6
            cou = cou - 3; // 10 -3 = 7 // 7-3 = 4 // 6-3 = 3
            cou = (cou - 1) / 3;

            int j = 1;
            List<BsonDocument> multiple = new List<BsonDocument>();
            for (int i = 0; i < cou; i++)
            {
                var record = new BsonDocument
                        {
                           { "uni_name"    , data.ElementAt(j++) },
                           { "pass_year"   , data.ElementAt(j++) },
                           { "degree_name" , data.ElementAt(j++) }
                        };

                multiple.Add(record);
            }
            var com = data.ElementAt(j++);
            var pos = data.ElementAt(j++);
            var proj = new BsonDocument
            {
                {"no_of_projects",int.Parse(data.ElementAt(j++)) }

            };
            MongoUser.add_edu_infos(multiple, id);
            MongoUser.upd_pro_info(com, pos, proj, 2, id);
            data.Clear();
            data.Add(id);
        }

        private bool check_status(int call)
        {
            if (id != null || occupation != null)
            {
                var Client = new MongoClient();
                var MongoDB = Client.GetDatabase("Botdatabase");
                var Collec = MongoDB.GetCollection<BsonDocument>("users");

                var info = new BsonDocument
                {
                    {"_id",data.ElementAt(0)},
                    {"basic",new BsonDocument
                     {
                        { "first_name" , data.ElementAt(1) },
                        { "last_name"  , data.ElementAt(2) },
                        { "age"        , "25" },//Convert.ToInt32(data.ElementAt(3)),
                        { "gender"     , data.ElementAt(4) },
                        { "location"   , data.ElementAt(5) },
                        { "email"      , data.ElementAt(6) },
                        { "status"     , 1               }
                     }
                   },
                    {"professional" , new BsonDocument
                     {
                        { "occupation_type" , occupation}
                     }
                    }
                };
                var chek = MongoUser.add_basic_info(info);
                occupation = null;
                flag = 0;
                data.Clear();
                data.Add(id);

                               
            }
            return false;
        }

        private string correctSequence(string intent, string reply, int x)
        {
            if (Enum.GetName(typeof(decision), counter) == intent)
            {
                reply = col.ElementAt(x);
                count++;
                counter++;
            }
            else
            {
                reply = "Sorry wrong input";
            }
            return reply;
        }

        
        protected void infoConfirm(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "Than save the user's basic information ?",
                Type = "postBack",
                Title = "Correct"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "Let's get to fixing your details. What's your first name ?",
                Type = "postBack",
                Title = "Incorrect",

            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void test_again(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "You want to give another language test",
                Type = "postBack",
                Title = "Yes"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "OK Thanks for visiting our bot",
                Type = "postBack",
                Title = "No",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }


        private void yesorno(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "Yes,What sort of company are you interested in working for:",
                Type = "postBack",
                Title = "Yes"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "What university did you attend ?",
                Type = "postBack",
                Title = "No",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void commands(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "your stored data has been displayed",
                Type = "postBack",
                Title = "View Profile"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "your stored scores has been displayed",
                Type = "postBack",
                Title = "View Test Scores",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "What university did you attend ?",
                Type = "postBack",
                Title = "Help",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void update_profile(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to update your basic details",
                Type = "postBack",
                Title = "Basic Info"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to update your educational information",
                Type = "postBack",
                Title = "Educational Info",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to update your professional information",
                Type = "postBack",
                Title = "Professional Info",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }


        private void selectdesign(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are designer of UX / UI",
                Type = "postBack",
                Title = "UX / UI"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are designer of Graphic / Web",
                Type = "postBack",
                Title = "Graphic / Web",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you are designer of Full Stack",
                Type = "postBack",
                Title = "Full Stack",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void test_type(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you want to give basic level test",
                Type = "postBack",
                Title = "Basic"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you want to give medium level test",
                Type = "postBack",
                Title = "Medium",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you want to give expert level test",
                Type = "postBack",
                Title = "Expert",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private string refferedtochoice(string id, Activity reply)
        {
            var result = MongoUser.exist_user(id);
            var occupy = JsonConvert.DeserializeObject<MongoData>(result.ToJson());
            string rep = null;
            switch (occupy.professional.occupation_type)
            {
                case "Product":
                      break;

                case "Design":
                    rep = col.ElementAt(14);
                    selectdesign(reply);
                    break;

                case "Development":
                    break;
                case "Marketing":
                    break;
                case "Sales":
                    break;
                case "Administration":
                    break;

            }

            return rep;
        }

        private void selectCompany(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are interested in Design/Development Studio",
                Type = "postBack",
                Title = "Design/Development Studio"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are interested in Financial Technology",
                Type = "postBack",
                Title = "Financial Technology"
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you are interested in Financial Technology",
                Type = "postBack",
                Title = "Game Studio"
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you are interested in ECommerce",
                Type = "postBack",
                Title = "ECommerce"
            };
            CardAction Button5 = new CardAction()
            {
                Value = "ok you are interested in Sales",
                Type = "postBack",
                Title = "Sales"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "ok you are interested in Female Focused Technology",
                Type = "postBack",
                Title = "Female Focused Technology"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void choose_tech(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Android",  //technology
                Type = "postBack",
                Title = "Android"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test IOS",
                Type = "postBack",
                Title = "IOS"
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Cross Platform",
                Type = "postBack",
                Title = "Cross Platform"
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Python",
                Type = "postBack",
                Title = "Python"
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test PHP",
                Type = "postBack",
                Title = "PHP"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test C#",
                Type = "postBack",
                Title = "C#"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }


        private void JobOptions(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "Product is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Product"

            };

            CardAction Button2 = new CardAction()
            {
                Value = "Design is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Design"

            };

            CardAction Button3 = new CardAction()
            {
                Value = "Development is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Development"
            };

            CardAction Button4 = new CardAction()
            {
                Value = "Marketing is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Marketing"
            };

            CardAction Button5 = new CardAction()
            {
                Value = "Sales is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Sales"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "Administration is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Administration"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }


        private void test(Activity reply, string opt, int mod)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            string[] option = opt.Split('@');
            CardAction Button1 = new CardAction()
            {
                Value = "A your are still in test mode" + mod,
                Type = "postBack",
                Title = option[0],
            };
            CardAction Button2 = new CardAction()
            {
                Value = "B your are still in test mode" + mod,
                Type = "postBack",
                Title = option[1],
            };
            CardAction Button3 = new CardAction()
            {
                Value = "C your are still in test mode" + mod,
                Type = "postBack",
                Title = option[2],
            };
            CardAction Button4 = new CardAction()
            {
                Value = "D your are still in test mode" + mod,
                Type = "postBack",
                Title = option[3],
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }
    }

    //internal class MemberDialog : IDialog<object>
    //{
    //    public async Task StartAsync(IDialogContext context)
    //    {
    //        string userChoice = "Hi how are you";
    //        await context.PostAsync(userChoice);
    //       // context.Wait(MessageReceived);
    //    }
    //}
}