const headerOption = (ShowRegisterModal, ShowLoginModal, Logouted) => {
    const isLoggedIn = inject('isLoggedIn');
    const currentState = inject('currentState');
    const headerLinks = reactive([
        {
            OrderNum: 1,state: [1,2], linktype: 'a', text: '找場地', link: `${getBaseUrl()}home/index`, className: 'nav-link text-dark', show: false
        },
        {
            OrderNum: 2, state: [1,2], linktype: 'a', text: '成為場地主', link: `${getBaseUrl()}home/PlaceManage`, className: 'nav-link text-dark', show: false
        },
        {
            OrderNum: 3, state: [1], linktype: 'a', text: '註冊', className: 'nav-link text-dark', show: false, onclick: ShowRegisterModal
        },
        {
            OrderNum: 4, state: [1], linktype: 'a', text: '登入', className: 'nav-link text-dark', show: false, onclick: ShowLoginModal
        },
        {
            OrderNum: 99, state: [2,3], linktype: 'dropdown', show: false, onclick: Logouted
        },
        {
            OrderNum: 5, state: [3], linktype: 'a', text: '返回管理頁', className: 'nav-link text-dark', show: false, link: `${getBaseUrl()}home/PlaceManage`
        },
    ]);

    const setShowType = (state) => {
        headerLinks.forEach(item => {
            if (item.state.includes(state)) {
                item.show = true;
            } else {
                item.show = false;
            }
        });
        // 根據 OrderNum 進行排序
        headerLinks.sort((a, b) => a.OrderNum - b.OrderNum);
    };

    // 監聽isLoggedIn的變化
    watch(isLoggedIn, (newValue) => {
        if (!newValue) {
            currentState.value = 1;
        } else {
            currentState.value = 2;
        }
    });

    // 監聽currentState的變化
    watch(currentState, (newState) => {
        setShowType(newState);
    });

    return {
        headerLinks,
        setShowType,
        currentState
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

            onBeforeMount(async () => {
                setShowType(currentState.value);
            });

            const {
                headerLinks, setShowType, currentState
            } = headerOption(ShowRegisterModal, ShowLoginModal, Logouted);

            return {
                headerLinks
            };
        }
    }

});

export default headerTemplate;