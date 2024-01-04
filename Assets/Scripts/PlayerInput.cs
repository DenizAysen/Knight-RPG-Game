using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //public FrameInput FrameInput { get; private set; }
    private FrameInput _frameInput;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _frameInput.SkillIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _frameInput.SkillIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _frameInput.SkillIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _frameInput.SkillIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _frameInput.SkillIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _frameInput.SkillIndex = 5;
        }
        else
        {
            _frameInput.SkillIndex = -1;
        }
    }
    //private FrameInput GatherInput()
    //{
    //    return FrameInput;
    //}
    public FrameInput GetInput()
    {
        return _frameInput;
    }
}
public struct FrameInput
{
    public int SkillIndex;
}
