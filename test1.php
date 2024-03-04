<!DOCTYPE html>
<html>
    <head>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    </head>
<body>

<div id="demo"></div>

<script>
function getIndex() {
  document.getElementById("demo").innerHTML =
  document.getElementById("mySelect").value;
}
</script>

<form>
Select your favorite fruit:
<select id="mySelect" onclick="user_notification()">
<option value="">Select...</option>
  <option>Apple</option>
  <option>Orange</option>
  <option>Pineapple</option>
  <option>Banana</option>
</select>
<br><br>
</form>

<script>
//var t = 5;
//setInterval(user_notification,t);
function user_notification()
{
    let v = document.getElementById("mySelect").value;
    if(v!="")
    {
    	jQuery.ajax({
    		url:'test.php',
    		type:'post',
    		data: { 'key' : '57','table': v},
    		success:function(result)
    		{
    			document.getElementById("demo").innerHTML = result;
    		}
    	});
    }

} 
</script>

</body>
</html>