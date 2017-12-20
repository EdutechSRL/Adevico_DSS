$(function(){
    $(".collapsable .expander").click(function(){
        $(this).parents(".collapsable").first().toggleClass("collapsed").toggleClass("expanded");
    });

});
