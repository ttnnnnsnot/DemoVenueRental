const indexBanner = () => {
    const bannerimg = ref(['../img/1.jpg', '../img/2.jpg']);
    const currentIndex = ref(0);
    const timeStart = ref(5000);
    
    const startImageSwitching = () => {
        if (bannerimg.value.length <= 1)
            return;

        setInterval(() => {
            currentIndex.value = (currentIndex.value + 1) % bannerimg.value.length;
        }, timeStart.value); // 每秒切换一次
    }

    const setImageStyle = (image) => {
        return {
            backgroundImage: `url(${image})`,
        }
    }

    return {
        bannerimg,
        currentIndex,
        startImageSwitching,
        setImageStyle
    }
}

const indexBannerOption = {
    template: '#indexBanner',
    setup() {
        const {
            bannerimg,
            currentIndex,
            startImageSwitching,
            setImageStyle
        } = indexBanner();

        return {
            bannerimg,
            currentIndex,
            startImageSwitching,
            setImageStyle
        };
    },
    mounted() {
        this.startImageSwitching();
    }
}

const backgroundImgChange = createApp();
backgroundImgChange.component('index-banner', indexBannerOption);
backgroundImgChange.mount("#backgroundImgChange");

/*
const backgroundImgChange = createApp({
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
*/