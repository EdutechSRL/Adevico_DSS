
$(document).ready(function() {
  
  function megaHoverOver(){
    $('select').blur();
    $(this).find(".sub").stop().fadeTo('fast', 1).show();    

    var subWidth = 0;

    if ($(this).find(".col").length > 0) { //If col exists...
			//Calculate each column
      $(this).find(".col:last").css({'margin-right':'0'});
			$(this).find(".col").each(function(){
			 subWidth += $(this).outerWidth(true);
			});
	  }
		else{
			subWidth = $(this).find("ul:first").outerWidth(true);
		}
		$(this).find(".sub").css({'width' : subWidth});
		
  }
  
  function megaHoverOut(){ 
    $(this).find(".sub").stop().fadeTo('fast', 0, function() {
      $(this).hide(); 
    });
  }

  var config = {    
     sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)    
     interval: 100, // number = milliseconds for onMouseOver polling interval    
     over: megaHoverOver, // function = onMouseOver callback (REQUIRED)    
     timeout: 200, // number = milliseconds delay before onMouseOut    
     out: megaHoverOut // function = onMouseOut callback (REQUIRED)    
  };

  $("#nav-main ul#top li .sub").hide().css({'opacity':'0'});
  $("#nav-main ul#top li").hoverIntent(config);

  $("#notifications ul li ul, #tools ul li ul").css({ 'opacity': '0' });
  $("#notifications ul:first li, #tools ul:first li").hoverIntent(config);
    
});
