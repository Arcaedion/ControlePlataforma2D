using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle2D : MonoBehaviour
{
    [SerializeField]
    private Transform _posicaoDeBaixo;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private float _suavizacaoMovimento = 0.05f;
    [Range(0,0.5f)]
    [SerializeField]
    private float _valorCortaPulo = 0.25f;
    [SerializeField]
    private float _forcaPulo = 300f;
    [SerializeField]
    private float _tempoDesdeChaoRef = 0.2f;

    private Rigidbody2D _rb;
    private bool _estaNoChao;
    private float _tempoDesdeChao;

    private bool _viradoParaDireita = true;
    private bool _controleAereo = true;

    const float _raioParaChao = 0.3f;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _tempoDesdeChao -= Time.deltaTime;
        if (_estaNoChao)
        {
            _tempoDesdeChao = _tempoDesdeChaoRef;
        }
    }

    void FixedUpdate()
    {
        _estaNoChao = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_posicaoDeBaixo.position, _raioParaChao, _layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                _estaNoChao = true;
        }

    }

    public bool GetEstaNoChao()
    {
        return _estaNoChao;
    }

    public void CortaPulo()
    {
        if(!_estaNoChao && _rb.velocity.y > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _valorCortaPulo);
        }
    }

    public void Movimento(float qtdMovimento, bool pulando)
    {
        if (_estaNoChao || _controleAereo)
        {
            AplicaMovimento(qtdMovimento);
            DetectaGirar(qtdMovimento);
        }

        if(pulando && _tempoDesdeChao > 0)
        {
            _estaNoChao = false;
            _rb.AddForce(new Vector2(0f, _forcaPulo));
        }

    }

    private void AplicaMovimento(float qtdMovimento)
    {
        Vector2 velocidadeJogador = new Vector2(qtdMovimento, _rb.velocity.y);
        Vector3 rapidez = Vector3.zero;
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, velocidadeJogador, ref rapidez, _suavizacaoMovimento);
    }

    private void DetectaGirar(float qtdMovimento)
    {
        if(qtdMovimento > 0 && !_viradoParaDireita)
        {
            transform.Rotate(0f, 180f, 0f);
            _viradoParaDireita = true;
        }
        else if(qtdMovimento < 0 && _viradoParaDireita)
        {
            transform.Rotate(0f, 180f, 0f);
            _viradoParaDireita = false;

        }
    }
}
