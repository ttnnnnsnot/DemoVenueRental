const backgroundImgChange = Vue.createApp({
    data() {
        return {
            bannerimg: ['../img/1.jpg', '../img/2.jpg'],
            currentIndex: 0,
        }
    },
    mounted() {
        this.startImageSwitching();
    },
    methods: {
        startImageSwitching() {
            setInterval(() => {
                this.currentIndex = (this.currentIndex + 1) % this.bannerimg.length;
            }, 5000); // 每秒切换一次
        },
        getImageStyle(image) {
            return {
                backgroundImage: `url(${image})`,
            };
        },
    },
});

backgroundImgChange.mount("#backgroundImgChange");