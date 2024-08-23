const indexBanner = () => {
    const bannerimg = ref(['../img/1.jpg', '../img/2.jpg']);
    const currentIndex = ref(0);
    const timeStart = ref(5000);

    const startImageSwitching = () => {
        if (bannerimg.value.length <= 1)
            return;

        setInterval(() => {
            currentIndex.value = (currentIndex.value + 1) % bannerimg.value.length;
        }, timeStart.value);
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
};

const indexBannerOption = defineAsyncComponent(async () => {
    return {
        template: await loadTemplate('/templates/IndexBanner.html'),
        setup() {
            const {
                bannerimg,
                currentIndex,
                startImageSwitching,
                setImageStyle
            } = indexBanner();

            onMounted(() => startImageSwitching());

            return {
                bannerimg,
                currentIndex,
                startImageSwitching,
                setImageStyle
            };
        }
    }
        
});

export default indexBannerOption;