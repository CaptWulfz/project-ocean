using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] MineAnimation mineAnim;
    private bool mineExplode = false;
    private Player player;
    public bool MineExplode
    {
        get { return this.mineExplode; }
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
            Debug.Log("If u see this u are ded");
            mineExplode = true;
            StartCoroutine(WaitForExplosion(collision.gameObject.GetComponent<Player>()));
        }
    }

    IEnumerator WaitForExplosion(Player player){
        Debug.Log("Helo");
        yield return new WaitForSeconds(1.5f);
        player.OnMineExplosionDead();
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
