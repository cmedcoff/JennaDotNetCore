using System.Globalization;

using RestSharp;

using Shared;

namespace AccountBalanceClient
{
    public partial class Form1 : Form
    {
        private string accountNumber;

        public Form1()
        {
            InitializeComponent();
            accountNumber = "1500";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        /*
         * Discuss 
         * avoid asyc void except for event handlers
         * async Task for "void-ish" async methods
         * async Task<T> for async methods that return a value
         */
        private async void timer1_Tick(object sender, EventArgs e)
        {
            // UpdateLabelWithCurrentTime();
            // GetAccountBalanceSynchronously();
            // GetAccountBalanceAsyncTaskParallelLibrary();
            // await GetAccountBalanceAsyncAwait();
            await GetMultipleAccountBalancesAsyncAwait();

            /*
             *  don't forget to offer the option of going down down the async rabbit hole
             *  by discussing what the compiler actually generates, e.g. are threads used, etc.
             *  
             *  Also remember when using TPS to do things in parallel, race conditions can occur
             *  and have to be dealt with the same way as when using threads directly
             *  
             *  The following blog post is a good resource for understanding async/await 
             *  
             *  https://blog.stephencleary.com/2012/02/async-and-await.html
             *
             *  and has many links to other resources as well that "go down the rabbit hole" including the
             *  state machine that the compiler generates here:
             *  
             *  https://learn.microsoft.com/en-us/archive/msdn-magazine/2011/october/asynchronous-programming-async-performance-understanding-the-costs-of-async-and-await
             *  
             *  threading aspects are covered in the book "Async in C# 5.0" by Alex Davies
             
             *  https://learning.oreilly.com/library/view/async-in-c/9781449337155/
             *  
             *  Microsoft has other async patterns that existed before async/await:
             *  
             *  See https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/
             *  
             *  I owe you an update to this to demonstrate using threading
             *  
             *  Pluralsight courses on async/await
             *  
             *  https://app.pluralsight.com/library/courses/intro-async-parallel-dotnet4/table-of-contents
             *  https://app.pluralsight.com/library/courses/getting-started-with-asynchronous-programming-dotnet/table-of-contents
             *  https://app.pluralsight.com/library/courses/c-sharp-10-asynchronous-programming/table-of-contents
             *  
             */
        }

        private void UpdateLabelWithCurrentTime()
        {
            this.label1.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void GetAccountBalanceSynchronously()
        {
            var client = new RestClient(new RestClientOptions("https://localhost:7086"));

            // synchronous API call using RestSharp library
            // slow or unresponsive API calls will freeze the UI
            // doesn't to be an API call, just the example here, could be any slow or unresponsive code
            // calls the the database, some CPU or IO intensive code, etc.
            ToggleAccountNumber();
            var request = new RestRequest("balance")
                .AddParameter("accountNumber", accountNumber, ParameterType.QueryString);
            RestResponse<AccountBalance> response = client.Execute<AccountBalance>(request);
            label1.Text = response.Data?.Balance.ToString(CultureInfo.InvariantCulture);
        }

        private void ToggleAccountNumber()
        {
            accountNumber = accountNumber == "1500" ? "1400" : "1500"; // toggle between two account numbers
        }

        private void GetAccountBalanceAsyncTaskParallelLibrary()
        {
            // one could use background threads, but that's a lot of work
            // not to mention the need to marshal the results back to the UI thread
            // complicates the code and introduces the possibility of bugs
            // plus the thread would most just wait tying up resources on the server
            // making the server less scalable, so use async instead which only
            // relies on I/O completion ports and doesn't tie up threads

            // async API call using RestSharp library via Task Parallel Library or TPL
            // TPL pre-dates async/await (and is still useful for some use cases even with async/await)
            // TPL is a bit more complicated than async/await
            //Task<RestResponse<AccountBalance>> task = null;
            ToggleAccountNumber();
            var client = new RestClient(new RestClientOptions("https://localhost:7086"));
            var request = new RestRequest("balance")
                .AddParameter("accountNumber", accountNumber, ParameterType.QueryString);

            // creates, immediately starts, and returns a task, may be done or not upon return
            Task<RestResponse<AccountBalance>> task =
                Task.Run(() => client.Execute<AccountBalance>(request));
            // setup continuation to run when task completes
            task.ContinueWith(previousTask =>
                label1.Text = MyContinuation(previousTask),
                // a bunch other other optional parameters need here
                // to ensure that the continuation runs on the UI thread,
                // the only place we can make UI updates,
                // so this still required knowledge and can be tricky.
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext()
                );
        }

        private static string? MyContinuation(Task<RestResponse<AccountBalance>> previousTask)
        {
            return previousTask.Result.Data?.Balance.ToString(CultureInfo.InvariantCulture);
        }

        private async Task GetAccountBalanceAsyncAwait()
        {
            try
            {
                var client = new RestClient(new RestClientOptions("https://localhost:7086"));

                // synchronous API call using RestSharp library
                ToggleAccountNumber();
                var request = new RestRequest("balance")
                    .AddParameter("accountNumber", accountNumber, ParameterType.QueryString);
                var response = await client.ExecuteAsync<AccountBalance>(request);
                // this continuation runs on the UI thread without specifying anything
                // nearly identical to synchronous code, but doesn't block the UI thread
                label1.Text = response.Data?.Balance.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private async Task GetMultipleAccountBalancesAsyncAwait()
        {
            label1.Text = string.Empty;

            var accountNumbers = new[] { "1005", "1006", "1007" }; // could be dozens, hundreds from db, etc.
            var client = new RestClient(new RestClientOptions("https://localhost:7086"));

            // one after another, inefficient
            //foreach (var accountNumber in accountNumbers)
            //{
            //    var request = new RestRequest("balance")
            //        .AddParameter("accountNumber", accountNumber, ParameterType.QueryString);
            //    RestResponse<AccountBalance> responseInLoop = await client.ExecuteAsync<AccountBalance>(request);
            //    label1.Text += $"\n\n{responseInLoop.Data?.Balance.ToString(CultureInfo.InvariantCulture)}";
            //}

            var tasks = new List<Task<RestResponse<AccountBalance>>>();
            foreach (var loopAccountNumber in accountNumbers)
            {
                // note we are not starting the task here, we are just creating it

                tasks.Add(new Task<RestResponse<AccountBalance>>(() =>
                          {
                              var request = new RestRequest("balance")
                                  .AddParameter("accountNumber", loopAccountNumber, ParameterType.QueryString);
                              return client.Execute<AccountBalance>(request);
                          }));


                // altertative to demonstrate task that throws an exception
                // note here I'm wrapping the API call in a task so I can simulate a Task that throws an exception
                // when using the task parallel library
                //                tasks.Add(
                //                            new Task<RestResponse<AccountBalance>>(() =>
                //                            {
                //                                throw new Exception("test");
                //                                var request = new RestRequest("balance")
                //                                    .AddParameter("accountNumber", loopAccountNumber, ParameterType.QueryString);
                //                                return client.Execute<AccountBalance>(request);
                //                            }));


            }

            // start all the tasks
            tasks.ForEach(x => x.Start());

            // continue as long as our collection still has tasks
            while (tasks.Any())
            {
                try
                {
                    // returns when any one task completes
                    var completedTask = await Task.WhenAny(tasks);
                    // remove the completed task from the collection so that the while eventually exits
                    tasks.Remove(completedTask);

                    // one can check for a "faulted" task, or "observe" the exception
                    // by accessing the "Result" property of the task and using a try/catch
                    // note you must look for an AggregateException and loop as there could be
                    // multiple exceptions.  Catching only by Exception will work but you'll lose
                    // the details of the exception or exceptions

                    if (!completedTask.IsFaulted)
                    {
                        // accessing the "Result" property "observes" the exception
                        label1.Text += $"\n\n{completedTask.Result.Data?.Balance.ToString(CultureInfo.InvariantCulture)}";
                    }
                    else
                    {
                        // handle exception
                        if (completedTask.Exception is AggregateException x)
                            x.InnerExceptions.ToList().ForEach(x =>
                            {
                                System.Diagnostics.Debug.WriteLine(x.Message);
                            });
                    }
                }
                // see https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/exception-handling-task-parallel-library
                catch (AggregateException ex)
                {
                    foreach (var innerException in ex.InnerExceptions)
                    {
                        System.Diagnostics.Debug.WriteLine(innerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
