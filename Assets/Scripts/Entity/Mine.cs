using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] MineAnimation mineAnim;
    [SerializeField] Oxygen playerOxygen;
    [SerializeField] Player playerDie;
    private bool mineExplode = false;
    private Player player;
    public bool MineExplode
    {
        get { return this.mineExplode; }
        set { this.mineExplode= value; }
    }

    private void Start()
    {
        mineAnim.InitializeAnimator();
    }

    private void Update()
    {
        this.mineAnim.UpdateAnimator();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            player = collision.gameObject.GetComponent<Player>();
            playerOxygen = collision.gameObject.GetComponent<Oxygen>(); 
            Debug.Log("Triggered Mine");
            mineExplode = true;
            
            player.VisionCone = false;
            
            player.PlayerExplode = 1;
            StartCoroutine(mineAnim.WaitForAnimationToFinish("Mine_Explosion", () =>
            {
                Debug.Log("Waited for Mine Explosion");
                //playerOxygen.OxygenTimer = 0f;
                Destroy(this.gameObject);
                player.OnMineExplosionDead();
            }));
        }
    }

}
