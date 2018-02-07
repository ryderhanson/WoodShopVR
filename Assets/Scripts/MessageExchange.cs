using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MessageCallback = System.Action<object>;
using MessageActionList = System.Collections.Generic.List<MessageExchange.MessageCallbackWrapper>;
using MessageMap = System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<MessageExchange.MessageCallbackWrapper>>;


/// <summary>
/// A component that is kept and updated by the core game system
/// </summary>
public interface ISystemComponent
{
    void OnInit();
    void OnRemove();
    //An update function run manually by the system
    void PumpedUpdate();
}

/// <summary>
/// Facilitates uncoupled communication across all systems through subscription based messaging
/// ex: 
/// MessageExchange localReference = DependencyInjector.GetSystem<MessageExchange>()
/// localReference.Subscribe<MyEnum>(MyEnum.MyMessage, myPrivateClassCallback); /
/// localReference.Subscribe<MyEnum>(MyEnum.MyMessage, (payload) => { delet(payload) };, this); //note the this pointer, which is used to ensure the lamba is properly unsubscribed
/// ...
/// localReference.UnSubscribe<MyEnum>(MyEnum.MyMessage, myPrivateClassCallback); /
/// localReference.UnSubscribe<MyEnum>(MyEnum.MyMessage, this); //when unsubbing from a lambda, just pass the message and this pointer
/// 
/// ... elsewhere ...
/// localReference.Publish<MyEnum>(MyEnum.MyMessage, { woahpayload });
/// </summary>
public class MessageExchange : ISystemComponent
{
    Dictionary<System.Type, MessageMap> _masterActionList = new Dictionary<System.Type, MessageMap>();

    public struct MessageCallbackWrapper
    {
        public MessageCallback Callback;
        public object Context;

        public MessageCallbackWrapper(MessageCallback mc, object context)
        {
            Callback = mc;
            Context = context;
        }
    }

    public void OnInit()
    {
        ///noop
    }

    public void OnRemove()
    {
        ///noop
    }

    public void PumpedUpdate()
    {
        ///noop
    }

    /// <summary>
    /// Subscribes to an enum-based message, please remember to UNSUBSCRIBE
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="messageType">An enum type</param>
    /// <param name="onMessageFired">Fired when message is sent with generic 'object'</param>
    /// <param name="context">'this' pointer from caller, extremely important parameter when passing lambdas to allow proper unsubscribing</param>
    public void Subscribe<EnumType>(EnumType messageType, MessageCallback onMessageFired, object context = null) where EnumType : struct, System.IComparable, System.IConvertible, System.IFormattable
    {
        if (onMessageFired != null)
        {
            MessageMap mMap = null;
            if (!_masterActionList.TryGetValue(typeof(EnumType), out mMap))
            {
                mMap = new MessageMap();

                _masterActionList.Add(typeof(EnumType), mMap);
            }

            MessageActionList list = null;
            if (!mMap.TryGetValue(messageType, out list))
            {
                list = new MessageActionList();

                mMap.Add(messageType, list);
            }

            list.Add(new MessageCallbackWrapper(onMessageFired, context));
        }
        else
        {
            Debug.LogError("[MessageExchange] Can't subscribe with a null callback");
        }
    }
    /// <summary>
    /// Unsubscribes from a message with a lambda callback. Very important for performance and memory
    /// </summary>
    /// <typeparam name="EnumType"></typeparam>
    /// <param name="messageType"></param>
    /// <param name="context">this pointer of caller</param>
    public void UnSubscribe<EnumType>(EnumType messageType, object context) where EnumType : struct, System.IComparable, System.IConvertible, System.IFormattable
    {
        MessageCallbackWrapper toRemove = default(MessageCallbackWrapper);
        bool found = false;

        MessageMap mMap = null;
        if (_masterActionList.TryGetValue(typeof(EnumType), out mMap))
        {
            MessageActionList mList = null;
            if (mMap.TryGetValue(messageType, out mList))
            {
                foreach (MessageCallbackWrapper mCallback in mList)
                {
                    if (mCallback.Context == context)
                    {
                        toRemove = mCallback;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    mList.Remove(toRemove);
                }
            }
        }
    }

    public void UnSubscribe<EnumType>(EnumType messageType, MessageCallback calllback) where EnumType : struct, System.IComparable, System.IConvertible, System.IFormattable
    {
        MessageCallbackWrapper toRemove = default(MessageCallbackWrapper);
        bool found = false;

        MessageMap mMap = null;
        if (_masterActionList.TryGetValue(typeof(EnumType), out mMap))
        {
            MessageActionList mList = null;
            if (mMap.TryGetValue(messageType, out mList))
            {
                foreach (MessageCallbackWrapper mCallback in mList)
                {
                    if (mCallback.Callback == calllback)
                    {
                        toRemove = mCallback;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    mList.Remove(toRemove);
                }
            }
        }
    }

    /// <summary>
    /// Publish to trigger all subscribed callbacks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="messageType"></param>
    /// <param name="payload"></param>
    public void Publish<EnumType>(EnumType messageType, object payload) where EnumType : struct, System.IComparable, System.IConvertible, System.IFormattable
    {
        MessageMap mMap = null;
        if (_masterActionList.TryGetValue(typeof(EnumType), out mMap))
        {
            MessageActionList actionList = null;
            if (mMap.TryGetValue(messageType, out actionList))
            {
                foreach (MessageCallbackWrapper callbackWrap in actionList)
                {
                    callbackWrap.Callback(payload);
                }
            }
        }
    }
}

