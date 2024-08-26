const headerOption = (ShowRegisterModal, ShowLoginModal, Logouted) => {
    const isLoggedIn = inject('isLoggedIn');
    const currentState = ref(1); // 初始狀態為state1
    const headerLinks = reactive([
        {
            linktype: 'a', text: '找場地', link: `${getBaseUrl()}home/index`, className: 'nav-link text-dark', show: true
        },
        {
            linktype: 'a', text: '成為場地主', link: `${getBaseUrl()}home/test`, className: 'nav-link text-dark', show: true
        },
        {
            linktype: 'a', text: '註冊', className: 'nav-link text-dark', show: true, onclick: ShowRegisterModal
        },
        {
            linktype: 'a', text: '登入', className: 'nav-link text-dark', show: true, onclick: ShowLoginModal
        },
        {
            linktype: 'dropdown', show: false, onclick: Logouted
        }
    ]);

    const setShowType = (state) => {
        switch (state) {
            case 1:
                headerLinks[0].show = true;
                headerLinks[1].show = true;
                headerLinks[2].show = true;
                headerLinks[3].show = true;
                headerLinks[4].show = false;
                break;
            case 2:
                headerLinks[0].show = true;
                headerLinks[1].show = true;
                headerLinks[2].show = false;
                headerLinks[3].show = false;
                headerLinks[4].show = true;
                break;
            default:
                // 如果狀態不匹配，則隱藏所有連結
                headerLinks.forEach(item => {
                    item.show = false;
                });
                break;
        }
    };

    const onMounted = async () => {
        try {
            isLoggedIn.value = await IsLoggedIn();
            setShowType(currentState.value);
        } catch (error) {
            console.error(error);
        }
    };

    // 監聽isLoggedIn的變化
    watch(isLoggedIn, (newValue) => {
        if (!newValue) {
            currentState.value = 1;
        } else {
            currentState.value = 2;
        }
        setShowType(currentState.value);
    });

    // 監聽currentState的變化
    watch(currentState, (newState) => {
        setShowType(newState);
    });

    return {
        isLoggedIn,
        currentState,
        headerLinks,
        setShowType,
        onMounted,
    };
};

const headerTemplate = defineAsyncComponent(async () => {
    return {
        template: await API.GetTemplate('templates/HeaderLinks.html'),
        emits: ['show-login', 'show-register', 'logouted'],
        setup(props, { emit }) {

            const ShowRegisterModal = () => {
                emit('show-register');
            };

            const ShowLoginModal = () => {
                emit('show-login');
            };

            const Logouted = () => {
                emit('logouted');
            }

            const {
                headerLinks,
                onMounted: headerOnMounted,
            } = headerOption(ShowRegisterModal, ShowLoginModal, Logouted);

            onMounted(async () => headerOnMounted());

            return {
                headerLinks,
            };
        }
    }

});

export default headerTemplate;