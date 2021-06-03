using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {
	protected SpriteRenderer m_sprite;
	public Sprite[] m_clips;
	public float m_timerPerFrame = 0.1f;
	public string m_spriteNamePrefix;

	protected float m_timer;
	protected int m_frameCnt = 0;

	// Use this for initialization
	void Start () {
		m_sprite = this.gameObject.GetComponent<SpriteRenderer> ();
		m_sprite.sprite = m_clips [m_frameCnt];
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		if (m_timer <= 0) {
			m_timer = this.m_timerPerFrame;
			m_frameCnt++;
			if (m_frameCnt >= m_clips.Length) {
				m_frameCnt = 0;
			}

			m_sprite.sprite = m_clips [m_frameCnt];
		}
	}
}
