$('.toggle').click(function (e) {
    e.preventDefault();

    var $this = $(this);

    //this is the close section
    if ($this.next().hasClass('show') ) {
        $this.next().removeClass('show');
        $this.next().slideUp(350);
    } else { //This is the open section
        $this.parent().parent().find('li div .inner').removeClass('show');
        $this.next().toggleClass('show');
        $this.next().slideToggle(350);
    }
});