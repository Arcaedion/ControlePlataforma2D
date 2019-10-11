using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controle2D))]
public class Movimento2DJogador : MonoBehaviour
{
    [SerializeField]
    private float _velocidade;
    [SerializeField]
    private Animator _animator;

    private Controle2D _controle;
    private float _movimentoHorizontal;
    private bool _pulando;
    void Awake()
    {
        _controle = GetComponent<Controle2D>();
    }

    void Update()
    {
        _movimentoHorizontal = Input.GetAxisRaw("Horizontal") * _velocidade;

        if (Input.GetButtonDown("Jump"))
        {
            _pulando = true;
        }

        if (_controle.GetEstaNoChao())
        {
            _animator.SetFloat("Velocidade", Mathf.Abs(_movimentoHorizontal));
            _animator.SetBool("Pulando", false);
        }
        else
        {
            _animator.SetBool("Pulando", true);

        }

    }

    private void FixedUpdate()
    {
        _controle.Movimento(_movimentoHorizontal * Time.fixedDeltaTime, _pulando);
        _pulando = false;
    }
}
