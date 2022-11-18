function f(x: int): void{
	log(fu(fu(x)));
}

function fu(y: int): int{
	return y + y;
}

f(5);
