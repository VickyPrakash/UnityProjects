using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeSharpPlus
{
    public class CreateRandom : NodeGroup
    {
        private int nodeN;
        public CreateRandom(params Node[] children)
            : base(children)
        {
            nodeN = 0;
            foreach (Node node in this.Children)
            {
                nodeN++;
            }
        }
        public override IEnumerable<RunStatus> Execute()
        {
            System.Random random = new System.Random();
            int chosen = random.Next(1, 1 + nodeN);
            int n = 0;
            foreach (Node node in this.Children)
            {
                n++;
                if (n == chosen)
                {
                    this.Selection = node;
                    node.Start();

                    RunStatus result;
                    while ((result = this.TickNode(node)) == RunStatus.Running)
                        yield return RunStatus.Running;
                    node.Stop();

                    this.Selection.ClearLastStatus();
                    this.Selection = null;

                    yield return result;
                    yield break;

                }

                yield return RunStatus.Success;
                yield break;
            }
        }
    }   
}
