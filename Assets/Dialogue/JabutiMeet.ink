-> inicio
#MARACAJÁ ESTÁ SOZINHO
=== inicio ===
#speaker:Jabuti
#portrait:JabutiNeutral
#audio:JabutiVoice
Olá, Gato-Maracajá. Vejo que também está tentando sair desta floresta... Será que posso me juntar a você?

* [Sim] -> sim
* [Não] -> nao

=== sim ===

#speaker:Maracajá
#portrait:MaracajaNeutral
#audio:MaracajaVoice
Hã... tá bom, pode vir, mas fique perto e não me atrapalhe, entendeu? Eu quero sair daqui o mais rápido possível!

#speaker:Jabuti
#portrait:JabutiNeutral
#audio:JabutiVoice
Não se preocupe, meu jovem. Caminharemos juntos e com calma. #outcome:jabuti_segue

-> END

=== nao ===

#speaker:Maracajá
#portrait:MaracajaNeutral
#audio:MaracajaVoice
Olha, Jabuti, me desculpe, mas eu não posso perder tempo esperando alguém lento como você.

#speaker:Jabuti
#portrait:JabutiNeutral
#audio:JabutiVoice
Jovem, todos estamos em perigo. A pressa nem sempre leva ao destino certo.

#speaker:Maracajá
#portrait:MaracajaNeutral
#audio:MaracajaVoice
Eu não quero ouvir lição agora! Se eu ficar aqui, algo vai me pegar! Tchau!

#speaker:Jabuti
#portrait:JabutiNeutral
#audio:JabutiVoice
Que o caminho te leve em segurança, mesmo que ande sozinho. #outcome:jabuti_nao_segue

-> END
