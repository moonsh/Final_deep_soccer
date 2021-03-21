using System;
using XNode;

[Serializable]
public class InOutValue
{
    [Node.Input] public BTContext inputContext;
    [Node.Output] public BTResult outResult;
}
