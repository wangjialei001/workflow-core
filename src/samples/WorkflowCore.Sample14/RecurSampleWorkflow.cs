using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Sample14
{
    class RecurSampleWorkflow : IWorkflow<MyData>
    {
        public string Id => "recur-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith(context => Console.WriteLine("Hello"))
                .Recur(data => TimeSpan.FromSeconds(5), data => data.Counter > 5).Do(recur => recur
                    .StartWith(context =>
                    {
                        Console.WriteLine("Doing recurring task");
                    })
                    .Then<AddNumbers1>().Input(step => step.value, step => step.Counter).Output(step => step.Counter, step => step.value)
                )
                .Then(context => Console.WriteLine("Carry on"));
        }
    }
    public class AddNumbers1 : StepBodyAsync
    {
        public int value { get; set; }
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            value = value + 1;
            return ExecutionResult.Next();
        }
    }
    public class MyData
    {
        public int Counter { get; set; }
    }
}
