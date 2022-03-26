using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;
using TMPro;

using System;


public class MqttController : MonoBehaviour
{
	private MqttClient client;
	public Button _sendButton;
	public TextMeshProUGUI _topic;
	public TextMeshProUGUI _message;

	private string topic;


	// Use this for initialization
	void Start()
	{
		_sendButton.onClick.AddListener(sendButtonClicked);
		// create client instance 
		client = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

		string clientId = Guid.NewGuid().ToString();
		client.Connect(clientId);

		// subscribe to the topic "/home/temperature" with QoS 2 
	}

	private void sendButtonClicked()
	{
		topic = _topic.text;

		var message = _message.text;

		subscribe(topic);
		publish(message);

		Debug.Log(topic);
	}

	private void subscribe(string topic)
	{
		client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}

	private void publish(string message)
    {
		client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

	private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
	{

		Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
		{
			Debug.Log("sending...");
			client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}
	}
	// Update is called once per frame
	void Update()
	{



	}
}
